using AviationSalon.Core.Abstractions.Repositories;
using AviationSalon.Core.Abstractions.Services;
using AviationSalon.Core.Data.Entities;
using Microsoft.Extensions.Logging;

namespace AviationSalon.App.Services
{
    public class WeaponService : IWeaponService
    {
        private readonly IRepository<WeaponEntity> _weaponRepository;
        private readonly ILogger<WeaponService> _logger;

        public WeaponService(IRepository<WeaponEntity> weaponRepository, ILogger<WeaponService> logger)
        {
            _weaponRepository = weaponRepository;
            _logger = logger;
        }

        public async Task<List<WeaponEntity>> GetWeaponsListAsync()
        {
            try
            {
                var weapons = await _weaponRepository.GetAllAsync();
                _logger.LogInformation($"Getting the list of weapons with count: {weapons.Count()}.");
                return weapons;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting the list of weapons. Details: {ex.Message}");
                throw;
            }
        }

        public async Task<WeaponEntity> GetWeaponDetailsAsync(string weaponId)
        {
            try
            {
                _logger.LogInformation($"Getting details for weapon with ID: {weaponId}.");
                return await _weaponRepository.GetByIdAsync(weaponId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting details for weapon. Details: {ex.Message}");
                throw;
            }
        }       
    }
}
