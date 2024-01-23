using AviationSalon.Core.Data.Entities;
using AviationSalon.Core.Data.Enums;
using AviationSalon.Infrastructure;
using AviationSalon.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace AviationSalon.Tests.Repositories
{

    public class WeaponRepositoryTests : IDisposable
    {
        private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;
        private readonly ApplicationDbContext _dbContext;

        private WeaponRepository _weaponRepository;
        private WeaponEntity _existingWeapon;
        private WeaponEntity _nonExistingWeapon;

        public WeaponRepositoryTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new ApplicationDbContext(_dbContextOptions);

            _weaponRepository = new WeaponRepository(_dbContext);

            _existingWeapon = new WeaponEntity { Name = "TestWeapon", Type = WeaponType.AirToAir, FirePower = 100 };
            _nonExistingWeapon = new WeaponEntity { WeaponId = 123, Name = "NonExistingWeapon", Type = WeaponType.AirToAir, FirePower = 100 };
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        [Fact]
        public async Task AddAsync_ShouldAddWeaponToDatabase()
        {
            // Arrange & Act
            await _weaponRepository.AddAsync(_existingWeapon);

            // Assert
            var weaponFromDb = await _dbContext.Weapons.FindAsync(_existingWeapon.WeaponId);
            weaponFromDb.Should().NotBeNull();
            weaponFromDb.Name.Should().Be(_existingWeapon.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCorrectWeapon()
        {
            // Arrange
            await _dbContext.Weapons.AddAsync(_existingWeapon);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _weaponRepository.GetByIdAsync(_existingWeapon.WeaponId);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be("TestWeapon");
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateWeaponInDatabase()
        {
            // Arrange
            await _dbContext.Weapons.AddAsync(_existingWeapon);
            await _dbContext.SaveChangesAsync();

            // Act
            _existingWeapon.Name = "UpdatedWeapon";
            await _weaponRepository.UpdateAsync(_existingWeapon);

            // Assert
            var updatedWeapon = await _dbContext.Weapons.FindAsync(_existingWeapon.WeaponId);
            updatedWeapon.Should().NotBeNull();
            updatedWeapon.Name.Should().Be("UpdatedWeapon");
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteWeaponFromDatabase()
        {
            // Arrange
            await _dbContext.Weapons.AddAsync(_existingWeapon);
            await _dbContext.SaveChangesAsync();

            // Act
            await _weaponRepository.DeleteAsync(_existingWeapon);

            // Assert
            var deletedWeapon = await _dbContext.Weapons.FindAsync(_existingWeapon.WeaponId);
            deletedWeapon.Should().BeNull();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNullForNonExistingWeapon()
        {
            // Act
            var result = await _weaponRepository.GetByIdAsync(123);

            // Assert
            result.Should().BeNull();
        }

        [Theory]
        [InlineData(123)]
        [InlineData(456)]
        public async Task UpdateAsync_ShouldThrowExceptionForNonExistingWeapon(int nonExistingWeaponId)
        {
            // Arrange
            _nonExistingWeapon.WeaponId = nonExistingWeaponId;

            // Act & Assert
            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => _weaponRepository.UpdateAsync(_nonExistingWeapon));
        }

        [Theory]
        [InlineData(123)]
        [InlineData(456)]
        public async Task DeleteAsync_ShouldThrowExceptionForNonExistingWeapon(int nonExistingWeaponId)
        {
            // Arrange
            _nonExistingWeapon.WeaponId = nonExistingWeaponId;

            // Act & Assert
            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => _weaponRepository.DeleteAsync(_nonExistingWeapon));
        }
    }
}
