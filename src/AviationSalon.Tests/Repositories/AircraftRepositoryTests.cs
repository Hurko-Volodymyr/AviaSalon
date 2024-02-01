using AviationSalon.Core.Data.Entities;
using AviationSalon.Infrastructure.Repositories;
using AviationSalon.Infrastructure;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;

namespace AviationSalon.Tests.Repositories
{
    public class AircraftRepositoryTests : IDisposable
    {
        private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;
        private readonly ApplicationDbContext _dbContext;
        private readonly AircraftRepository _aircraftRepository;

        public AircraftRepositoryTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new ApplicationDbContext(_dbContextOptions);
            _aircraftRepository = new AircraftRepository(_dbContext);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        private AircraftEntity _aircraft { get; set; } = new AircraftEntity
        {
            AircraftId = "1",
            Model = "TestModel",
            Role = Core.Data.Enums.Role.Fighter,
            Range = 10,
            MaxHeight = 100,
            MaxWeaponsCapacity = 2,            
        };

        private AircraftEntity _nonExistingAircraft { get; set; } = new AircraftEntity
        {
            AircraftId = "-1222",
            Model = "NonExistingAircraft",
        };

        [Fact]
        public async Task AddAsync_ShouldAddAircraftToDatabase()
        {
            // Act
            await _aircraftRepository.AddAsync(_aircraft);

            // Assert
            var aircraftFromDb = await _dbContext.Aircrafts.FindAsync(_aircraft.AircraftId);
            aircraftFromDb.Should().NotBeNull();
            aircraftFromDb.Model.Should().Be(_aircraft.Model);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCorrectAircraft()
        {
            // Arrange
            await _dbContext.Aircrafts.AddAsync(_aircraft);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _aircraftRepository.GetByIdAsync(_aircraft.AircraftId);

            // Assert
            result.Should().NotBeNull();
            result.Model.Should().Be(_aircraft.Model);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateAircraftInDatabase()
        {
            // Arrange
            await _dbContext.Aircrafts.AddAsync(_aircraft);
            await _dbContext.SaveChangesAsync();

            // Act
            _aircraft.Model = "UpdatedModel";
            await _aircraftRepository.UpdateAsync(_aircraft);

            // Assert
            var updatedAircraft = await _dbContext.Aircrafts.FindAsync(_aircraft.AircraftId);
            updatedAircraft.Should().NotBeNull();
            updatedAircraft.Model.Should().Be("UpdatedModel");
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteAircraftFromDatabase()
        {
            // Arrange
            await _dbContext.Aircrafts.AddAsync(_aircraft);
            await _dbContext.SaveChangesAsync();

            // Act
            await _aircraftRepository.DeleteAsync(_aircraft);

            // Assert
            var deletedAircraft = await _dbContext.Aircrafts.FindAsync(_aircraft.AircraftId);
            deletedAircraft.Should().BeNull();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNullForNonExistingAircraft()
        {
            // Act
            var result = await _aircraftRepository.GetByIdAsync(_nonExistingAircraft.AircraftId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowExceptionForNullAircraftId()
        {
            // Arrange
            var aircraftWithNullId = new AircraftEntity { AircraftId = null };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _aircraftRepository.UpdateAsync(aircraftWithNullId));
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowExceptionForNonExistingAircraft()
        {            
            // Act & Assert
            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => _aircraftRepository.UpdateAsync(_nonExistingAircraft));
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowExceptionForNullAircraftId()
        {
            // Arrange
            var aircraftWithNullId = new AircraftEntity { AircraftId = null };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _aircraftRepository.DeleteAsync(aircraftWithNullId));
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowExceptionForNonExistingAircraft()
        {
            // Act & Assert
            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => _aircraftRepository.DeleteAsync(_nonExistingAircraft));
        }

    }

}
