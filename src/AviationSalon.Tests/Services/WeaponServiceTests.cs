using AviationSalon.App.Services;
using AviationSalon.Core.Abstractions.Repositories;
using AviationSalon.Core.Data.Entities;
using AviationSalon.Core.Data.Enums;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AviationSalon.Tests.Services
{
    public class WeaponServiceTests : IDisposable
    {
        private readonly Mock<IRepository<WeaponEntity>> _weaponRepositoryMock;
        private readonly Mock<ILogger<WeaponService>> _loggerMock;
        private readonly WeaponService _weaponService;

        public WeaponServiceTests()
        {
            _weaponRepositoryMock = new Mock<IRepository<WeaponEntity>>();
            _loggerMock = new Mock<ILogger<WeaponService>>();
            _weaponService = new WeaponService(_weaponRepositoryMock.Object, _loggerMock.Object);
        }

        public void Dispose()
        {
            _weaponRepositoryMock.Reset();
            _loggerMock.Reset();
        }

        [Fact]
        public async Task GetWeaponsListAsync_ShouldReturnListOfWeapons()
        {
            // Arrange
            _weaponRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<WeaponEntity>
        {
            new WeaponEntity { WeaponId = 1, Name = "Weapon1", Type = WeaponType.AirToAir, FirePower = 100 },
            new WeaponEntity { WeaponId = 2, Name = "Weapon2", Type = WeaponType.AirToGround, FirePower = 150 }
        });

            // Act
            var result = await _weaponService.GetWeaponsListAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result[0].Name.Should().Be("Weapon1");
            result[1].Name.Should().Be("Weapon2");
        }

        [Fact]
        public async Task GetWeaponDetailsAsync_ShouldReturnWeaponDetails()
        {
            // Arrange
            var weaponId = 1;
            var expectedWeapon = new WeaponEntity { WeaponId = weaponId, Name = "TestWeapon", Type = WeaponType.AirToAir, FirePower = 100 };
            _weaponRepositoryMock.Setup(repo => repo.GetByIdAsync(weaponId)).ReturnsAsync(expectedWeapon);

            // Act
            var result = await _weaponService.GetWeaponDetailsAsync(weaponId);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedWeapon);
        }

        [Fact]
        public async Task GetWeaponsListAsync_ShouldLogErrorAndThrowExceptionOnRepositoryFailure()
        {
            // Arrange
            _weaponRepositoryMock.Setup(repo => repo.GetAllAsync()).ThrowsAsync(new Exception("Repository error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _weaponService.GetWeaponsListAsync());
        }

        [Fact]
        public async Task GetWeaponDetailsAsync_ShouldLogErrorAndThrowExceptionOnRepositoryFailure()
        {
            // Arrange
            var weaponId = 1;
            var expectedExceptionMessage = "Repository error";

            _weaponRepositoryMock.Setup(repo => repo.GetByIdAsync(weaponId)).ThrowsAsync(new Exception(expectedExceptionMessage));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _weaponService.GetWeaponDetailsAsync(weaponId));

        }



    }

}
