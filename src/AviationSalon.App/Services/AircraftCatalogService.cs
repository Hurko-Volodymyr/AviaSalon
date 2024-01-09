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
            _logger.LogInformation("Getting the list of aircraft.");
            var aircraftEntities = await _aircraftRepository.GetAllAsync();
            return aircraftEntities.ToList();
        }

        public async Task<AircraftEntity> GetAircraftDetailsAsync(int aircraftId)
        {
            _logger.LogInformation($"Getting details for aircraft with ID: {aircraftId}.");
            var aircraftEntity = await _aircraftRepository.GetByIdAsync(aircraftId);
            return aircraftEntity;
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

                if (aircraft.Weapons.Count > aircraft.MaxWeaponsCapacity)
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
    }


}
