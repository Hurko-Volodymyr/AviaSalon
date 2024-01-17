using AviationSalon.Core.Data.Entities;

namespace AviationSalon.Core.Abstractions.Services
{
    public interface IWeaponService
    {
        Task<List<WeaponEntity>> GetWeaponsListAsync();
        Task<WeaponEntity> GetWeaponDetailsAsync(int weaponId);       
    }

}
