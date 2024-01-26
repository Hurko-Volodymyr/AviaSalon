using AviationSalon.Core.Abstractions.Repositories;
using AviationSalon.Core.Abstractions.Services;
using AviationSalon.Core.Data.Entities;
using Microsoft.Extensions.Logging;

namespace AviationSalon.App.Services
{
    public class AircraftCatalogService : IAircraftCatalogService
    {
        private readonly IRepository<AircraftEntity> _aircraftRepository;
        private readonly IRepository<WeaponEntity> _weaponRepository;
        private readonly ILogger<AircraftCatalogService> _logger;

        public AircraftCatalogService(
            IRepository<AircraftEntity> aircraftRepository,
            IRepository<WeaponEntity> weaponRepository,
            ILogger<AircraftCatalogService> logger)
        {
            _aircraftRepository = aircraftRepository;
            _weaponRepository = weaponRepository;
            _logger = logger;
        }

        public async Task<List<AircraftEntity>> GetAircraftListAsync()
        {
            try
            {               
                var aircraftEntities = await _aircraftRepository.GetAllAsync();
                _logger.LogInformation($"Getting the list of aircrafts with count:{aircraftEntities.Count()}.");
                return aircraftEntities;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting the list of aircraft. Details: {ex.Message}");
                throw;
            }
        }

        public async Task<AircraftEntity> GetAircraftDetailsAsync(int aircraftId)
        {
            try
            {
                _logger.LogInformation($"Getting details for aircraft with ID: {aircraftId}.");
                var aircraftEntity = await _aircraftRepository.GetByIdAsync(aircraftId);
                return aircraftEntity;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting details for aircraft. Details: {ex.Message}");
                throw;
            }
        }


        public async Task EquipAircraftWithWeaponAsync(int aircraftId, int weaponId)
        {
            try
            {
                _logger.LogInformation($"Equipping aircraft with ID {aircraftId} with weapon ID {weaponId}.");

                var aircraft = await _aircraftRepository.GetByIdAsync(aircraftId);
                var weapon = await _weaponRepository.GetByIdAsync(weaponId);

                if (aircraft == null || weapon == null)
                {
                    _logger.LogError("Aircraft or weapon not found.");
                    return;
                }

                if (aircraft.Weapons.Count >= aircraft.MaxWeaponsCapacity)
                {
                    _logger.LogError("The aircraft has reached the maximum number of weapons.");
                    return;
                }

                aircraft.Weapons.Add(weapon);
                await _aircraftRepository.UpdateAsync(aircraft);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error equipping aircraft with weapon. Details: {ex.Message}");
                throw;
            }
        }


        public async Task ClearLoadedWeaponsAsync(int aircraftId)
        {
            try
            {
                _logger.LogInformation($"Clearing loaded weapons for aircraft with ID {aircraftId}.");

                var aircraft = await _aircraftRepository.GetByIdAsync(aircraftId);

                if (aircraft == null)
                {
                    _logger.LogError($"Aircraft with ID {aircraftId} not found.");
                    return;
                }

                aircraft.Weapons.Clear();
                await _aircraftRepository.UpdateAsync(aircraft);

                _logger.LogInformation($"Cleared loaded weapons for aircraft with ID {aircraftId}.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error clearing loaded weapons. Details: {ex.Message}");
                throw;
            }
        }

    }


}
