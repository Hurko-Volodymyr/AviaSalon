using AviationSalon.App.Services;
using AviationSalon.Core.Abstractions.Repositories;
using AviationSalon.Core.Data.Entities;
using AviationSalon.Core.Data.Enums;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace AviationSalon.Tests.Services
{
    public class AircraftCatalogServiceTests : IDisposable
    {
        private readonly Mock<IRepository<AircraftEntity>> _aircraftRepositoryMock = new Mock<IRepository<AircraftEntity>>();
        private readonly Mock<IRepository<WeaponEntity>> _weaponRepositoryMock = new Mock<IRepository<WeaponEntity>>();
        private readonly Mock<ILogger<AircraftCatalogService>> _loggerMock = new Mock<ILogger<AircraftCatalogService>>();

        private List<AircraftEntity> _aircraftEntities;
        private AircraftCatalogService _aircraftService;
        private AircraftEntity _aircraftEntity;
        private WeaponEntity _weapon;
        private WeaponEntity _secondWeapon;

        public AircraftCatalogServiceTests()
        {
            _aircraftService = new AircraftCatalogService(_aircraftRepositoryMock.Object, _weaponRepositoryMock.Object, _loggerMock.Object);
            _weapon = new WeaponEntity()
            {
                    WeaponId = 1,
                    Name = "TestWeapon1",
                    Type = WeaponType.AirToAir,
                    GuidedSystem = GuidedSystemType.Infrared,
                    Range = 1000,
                    FirePower = 50                
            };

            _secondWeapon = new WeaponEntity()
            {
                WeaponId = 2,
                Name = "TestWeapon2",
                Type = WeaponType.AirToAir,
                GuidedSystem = GuidedSystemType.Infrared,
                Range = 1000,
                FirePower = 50
            };

            _aircraftEntity = new AircraftEntity ()
            {
                AircraftId = 1,
                Model = "Model123",
                MaxWeaponsCapacity = 5,
                Weapons = new List<WeaponEntity>()
            };

            _aircraftEntities = new List<AircraftEntity>
            {  _aircraftEntity, };
        }

        public void Dispose()
        {
            _weaponRepositoryMock.Reset();
            _aircraftRepositoryMock.Reset();
            _loggerMock.Reset();
        }

        [Fact]
        public async Task GetAircraftListAsync_ShouldReturnListOfAircraftEntities()
        {
            // Arrange
            _aircraftRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(_aircraftEntities);

            // Act
            var result = await _aircraftService.GetAircraftListAsync();

            // Assert
            result.Should().BeEquivalentTo(_aircraftEntities);
        }

        [Fact]
        public async Task GetAircraftListAsync_ShouldThrowExceptionOnRepositoryFailure()
        {
            // Arrange
            _aircraftRepositoryMock.Setup(repo => repo.GetAllAsync()).ThrowsAsync(new Exception("Repository error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _aircraftService.GetAircraftListAsync());
        }

        [Fact]
        public async Task GetAircraftDetailsAsync_ShouldReturnAircraftDetails()
        {
            // Arrange
            _aircraftRepositoryMock.Setup(repo => repo.GetByIdAsync(_aircraftEntity.AircraftId)).ReturnsAsync(_aircraftEntity);

            // Act
            var result = await _aircraftService.GetAircraftDetailsAsync(_aircraftEntity.AircraftId);

            // Assert
            result.Should().NotBeNull();
            result.AircraftId.Should().Be(_aircraftEntity.AircraftId);
            result.Model.Should().Be(_aircraftEntity.Model);
        }

        [Fact]
        public async Task GetAircraftDetailsAsync_ShouldThrowExceptionOnRepositoryFailure()
        {
            // Arrange
            var expectedExceptionMessage = "Repository error";
            _aircraftRepositoryMock.Setup(repo => repo.GetByIdAsync(_aircraftEntity.AircraftId)).ThrowsAsync(new Exception(expectedExceptionMessage));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _aircraftService.GetAircraftDetailsAsync(_aircraftEntity.AircraftId));
        }

        [Fact]
        public async Task EquipAircraftWithWeaponAsync_ShouldEquipAircraftWithWeapon()
        {
            // Arrange
            _aircraftRepositoryMock.Setup(repo => repo.GetByIdAsync(_aircraftEntity.AircraftId)).ReturnsAsync(_aircraftEntity);
            _weaponRepositoryMock.Setup(repo => repo.GetByIdAsync(_weapon.WeaponId)).ReturnsAsync(_weapon);

            // Act
            await _aircraftService.EquipAircraftWithWeaponAsync(_aircraftEntity.AircraftId, _weapon.WeaponId);

            // Assert
            _aircraftEntity.Weapons.Should().Contain(_weapon);
            _aircraftRepositoryMock.Verify(repo => repo.UpdateAsync(_aircraftEntity), Times.Once);
        }


        [Fact]
        public async Task EquipAircraftWithWeaponAsync_ShouldEquipAircraftWithAnotherWeapon()
        {
            // Arrange
            _aircraftRepositoryMock.Setup(repo => repo.GetByIdAsync(_aircraftEntity.AircraftId)).ReturnsAsync(_aircraftEntity);
            _weaponRepositoryMock.Setup(repo => repo.GetByIdAsync(_weapon.WeaponId)).ReturnsAsync(_weapon);
            _weaponRepositoryMock.Setup(repo => repo.GetByIdAsync(_secondWeapon.WeaponId)).ReturnsAsync(_secondWeapon);
            await _aircraftService.EquipAircraftWithWeaponAsync(_aircraftEntity.AircraftId, _weapon.WeaponId);

            // Act
            await _aircraftService.EquipAircraftWithWeaponAsync(_aircraftEntity.AircraftId, _secondWeapon.WeaponId);

            // Assert 
            _aircraftEntity.Weapons.Should().Contain(_weapon);
            _aircraftEntity.Weapons.Should().Contain(_secondWeapon);
        }

        [Fact]
        public async Task EquipAircraftWithWeaponAsync_ShouldNotEquipIfAircraftNotFound()
        {
            // Arrange
            _aircraftRepositoryMock.Setup(repo => repo.GetByIdAsync(_aircraftEntity.AircraftId)).ReturnsAsync(_aircraftEntity);
            _aircraftRepositoryMock.Setup(repo => repo.GetByIdAsync(_aircraftEntity.AircraftId)).ReturnsAsync((AircraftEntity)null);

            // Act
            await _aircraftService.EquipAircraftWithWeaponAsync(_aircraftEntity.AircraftId, _weapon.WeaponId);

            // Assert
            _aircraftRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<AircraftEntity>()), Times.Never);
        }


        [Fact]
        public async Task EquipAircraftWithWeaponAsync_ShouldNotEquipIfWeaponNotFound()
        {
            // Arrange
            _aircraftRepositoryMock.Setup(repo => repo.GetByIdAsync(_aircraftEntity.AircraftId)).ReturnsAsync(_aircraftEntity);
            _weaponRepositoryMock.Setup(repo => repo.GetByIdAsync(_weapon.WeaponId)).ReturnsAsync((WeaponEntity)null);

            // Act
            await _aircraftService.EquipAircraftWithWeaponAsync(_aircraftEntity.AircraftId, _weapon.WeaponId);

            // Assert
            _aircraftRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<AircraftEntity>()), Times.Never);
        }

        [Fact]
        public async Task EquipAircraftWithWeaponAsync_ShouldNotEquipIfAircraftHasMaxWeaponsCapacity()
        {
            // Arrange
            _aircraftRepositoryMock.Setup(repo => repo.GetByIdAsync(_aircraftEntity.AircraftId)).ReturnsAsync(_aircraftEntity);
            _weaponRepositoryMock.Setup(repo => repo.GetByIdAsync(_weapon.WeaponId)).ReturnsAsync(_weapon);
            _weaponRepositoryMock.Setup(repo => repo.GetByIdAsync(_weapon.WeaponId)).ReturnsAsync(_weapon);
            await _aircraftService.EquipAircraftWithWeaponAsync(_aircraftEntity.AircraftId, _weapon.WeaponId);
            await _aircraftService.EquipAircraftWithWeaponAsync(_aircraftEntity.AircraftId, _weapon.WeaponId);

            // Act
            await _aircraftService.EquipAircraftWithWeaponAsync(_aircraftEntity.AircraftId, _secondWeapon.WeaponId);

            // Assert 
            _aircraftEntity.Weapons.Should().Contain(_weapon);
            _aircraftEntity.Weapons.Should().NotContain(_secondWeapon);
        }


        [Fact]
        public async Task EquipAircraftWithWeaponAsync_ShouldErrorOnException()
        {
            // Arrange
            _aircraftRepositoryMock.Setup(repo => repo.GetByIdAsync(_aircraftEntity.AircraftId)).ThrowsAsync(new Exception("Repository error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _aircraftService.EquipAircraftWithWeaponAsync(_aircraftEntity.AircraftId, _weapon.WeaponId));
        }

        [Fact]
        public async Task ClearLoadedWeaponsAsync_ShouldClearWeaponsForAircraft()
        {
            // Arrange
            _aircraftRepositoryMock.Setup(repo => repo.GetByIdAsync(_aircraftEntity.AircraftId)).ReturnsAsync(_aircraftEntity);
            _weaponRepositoryMock.Setup(repo => repo.GetByIdAsync(_weapon.WeaponId)).ReturnsAsync(_weapon);
            _weaponRepositoryMock.Setup(repo => repo.GetByIdAsync(_secondWeapon.WeaponId)).ReturnsAsync(_secondWeapon);
            await _aircraftService.EquipAircraftWithWeaponAsync(_aircraftEntity.AircraftId, _weapon.WeaponId);
            await _aircraftService.EquipAircraftWithWeaponAsync(_aircraftEntity.AircraftId, _secondWeapon.WeaponId);

            // Act
            await _aircraftService.ClearLoadedWeaponsAsync(_aircraftEntity.AircraftId);

            // Assert
            _aircraftEntity.Weapons.Should().BeEmpty();
        }

        [Fact]
        public async Task ClearLoadedWeaponsAsync_ShouldNotClearWeaponsForNonexistentAircraft()
        {
            // Arrange
            _aircraftRepositoryMock.Setup(repo => repo.GetByIdAsync(_aircraftEntity.AircraftId)).ReturnsAsync((AircraftEntity)null);

            // Act
            await _aircraftService.ClearLoadedWeaponsAsync(_aircraftEntity.AircraftId);

            // Assert
            _aircraftRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<AircraftEntity>()), Times.Never);
        }

        [Fact]
        public async Task ClearLoadedWeaponsAsync_ShouldErrorOnException()
        {
            // Arrange
            _aircraftRepositoryMock.Setup(repo => repo.GetByIdAsync(_aircraftEntity.AircraftId)).ThrowsAsync(new Exception("Repository error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _aircraftService.ClearLoadedWeaponsAsync(_aircraftEntity.AircraftId));
        }

    }

}
