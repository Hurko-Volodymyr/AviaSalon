using AviationSalon.Core.Data.Entities;

namespace AviationSalon.Core.Abstractions.Services
{
    public interface IAircraftCatalogService
    {
        Task<List<AircraftEntity>> GetAircraftListAsync();
        Task<AircraftEntity> GetAircraftDetailsAsync(int aircraftId);
        Task EquipAircraftWithWeaponAsync(int aircraftId, int weaponId);
        Task ClearLoadedWeaponsAsync(int aircraftId);
    }



}
