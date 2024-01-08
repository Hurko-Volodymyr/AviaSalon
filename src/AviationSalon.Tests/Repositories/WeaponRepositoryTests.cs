using AviationSalon.Core.Data.Entities;
using AviationSalon.Core.Data.Enums;
using AviationSalon.Infrastructure;
using AviationSalon.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AviationSalon.Tests.Repositories
{

    public class WeaponRepositoryTests : IDisposable
    {
        private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;
        private readonly ApplicationDbContext _dbContext;

        public WeaponRepositoryTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new ApplicationDbContext(_dbContextOptions);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        [Fact]
        public async Task AddAsync_ShouldAddWeaponToDatabase()
        {
            // Arrange
            var weaponRepository = new WeaponRepository(_dbContext);
            var weapon = new WeaponEntity { Name = "TestWeapon", Type = WeaponType.AirToAir, FirePower = 100 };

            // Act
            await weaponRepository.AddAsync(weapon);

            // Assert
            var weaponFromDb = await _dbContext.Weapons.FindAsync(weapon.WeaponId);
            weaponFromDb.Should().NotBeNull();
            weaponFromDb.Name.Should().Be("TestWeapon");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCorrectWeapon()
        {
            // Arrange
            var weaponRepository = new WeaponRepository(_dbContext);
            var weapon = new WeaponEntity { Name = "TestWeapon", Type = WeaponType.AirToAir, FirePower = 100 };
            await _dbContext.Weapons.AddAsync(weapon);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await weaponRepository.GetByIdAsync(weapon.WeaponId);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be("TestWeapon");
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateWeaponInDatabase()
        {
            // Arrange
            var weaponRepository = new WeaponRepository(_dbContext);
            var weapon = new WeaponEntity { Name = "TestWeapon", Type = WeaponType.AirToAir, FirePower = 100 };
            await _dbContext.Weapons.AddAsync(weapon);
            await _dbContext.SaveChangesAsync();

            // Act
            weapon.Name = "UpdatedWeapon";
            await weaponRepository.UpdateAsync(weapon);

            // Assert
            var updatedWeapon = await _dbContext.Weapons.FindAsync(weapon.WeaponId);
            updatedWeapon.Should().NotBeNull();
            updatedWeapon.Name.Should().Be("UpdatedWeapon");
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteWeaponFromDatabase()
        {
            // Arrange
            var weaponRepository = new WeaponRepository(_dbContext);
            var weapon = new WeaponEntity { Name = "TestWeapon", Type = WeaponType.AirToAir, FirePower = 100 };
            await _dbContext.Weapons.AddAsync(weapon);
            await _dbContext.SaveChangesAsync();

            // Act
            await weaponRepository.DeleteAsync(weapon);

            // Assert
            var deletedWeapon = await _dbContext.Weapons.FindAsync(weapon.WeaponId);
            deletedWeapon.Should().BeNull();
        }
    }


}
