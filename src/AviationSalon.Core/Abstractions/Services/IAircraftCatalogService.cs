using AviationSalon.Core.Data.Entities;

namespace AviationSalon.Core.Abstractions.Services
{
    public interface IAircraftCatalogService
    {
        Task<List<AircraftEntity>> GetAircraftListAsync();
        Task<AircraftEntity> GetAircraftDetailsAsync(string aircraftId);
        Task EquipAircraftWithWeaponAsync(string aircraftId, string weaponId);
        Task ClearLoadedWeaponsAsync(string aircraftId);
    }



}
