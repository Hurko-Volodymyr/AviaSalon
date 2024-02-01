using AviationSalon.App.Services;
using AviationSalon.Core.Abstractions.Repositories;
using AviationSalon.Core.Data.Entities;
using AviationSalon.Core.Data.Enums;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace AviationSalon.Tests.Services
{
    public class WeaponServiceTests : IDisposable
    {
        private readonly Mock<IRepository<WeaponEntity>> _weaponRepositoryMock;
        private readonly Mock<ILogger<WeaponService>> _loggerMock;
        private readonly WeaponService _weaponService;
        private readonly WeaponEntity _weapon;
        private readonly WeaponEntity _anotherWeapon;
        private readonly WeaponEntity _nonExistingWeapon;

        public WeaponServiceTests()
        {
            _weaponRepositoryMock = new Mock<IRepository<WeaponEntity>>();
            _loggerMock = new Mock<ILogger<WeaponService>>();
            _weaponService = new WeaponService(_weaponRepositoryMock.Object, _loggerMock.Object);
            _weapon = new WeaponEntity { WeaponId = "1", Name = "Weapon1", Type = WeaponType.AirToAir, FirePower = 100 };
            _anotherWeapon = new WeaponEntity { WeaponId = "2", Name = "Weapon2", Type = WeaponType.AirToGround, FirePower = 150 };
            _nonExistingWeapon = new WeaponEntity { WeaponId = null };
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
                _weapon,
                _anotherWeapon,
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
            var expectedWeapon = _weapon;
            _weaponRepositoryMock.Setup(repo => repo.GetByIdAsync(_weapon.WeaponId)).ReturnsAsync(expectedWeapon);

            // Act
            var result = await _weaponService.GetWeaponDetailsAsync(_weapon.WeaponId);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedWeapon);
        }

        [Fact]
        public async Task GetWeaponsListAsync_ShouldThrowExceptionOnRepositoryFailure()
        {
            // Arrange
            _weaponRepositoryMock.Setup(repo => repo.GetAllAsync()).ThrowsAsync(new Exception("Repository error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _weaponService.GetWeaponsListAsync());
        }

        [Fact]
        public async Task GetWeaponDetailsAsync_ShouldThrowExceptionOnRepositoryFailure()
        {
            // Arrange
            var expectedExceptionMessage = "Repository error";

            _weaponRepositoryMock.Setup(repo => repo.GetByIdAsync(_weapon.WeaponId)).ThrowsAsync(new Exception(expectedExceptionMessage));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _weaponService.GetWeaponDetailsAsync(_weapon.WeaponId));

        }
    }
}
