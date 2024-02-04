using AviationSalon.Core.Data.Enums;

namespace AviationSalon.Core.Data.Entities
{
    public class AircraftEntity
    {
        public string AircraftId { get; set; }
        public string Model { get; set; } = string.Empty;
        public int Range { get; set; }
        public int MaxHeight { get; set; }
        public Role Role { get; set; }
        public string ImageFileName { get; set; } = string.Empty;
        public List<WeaponEntity> Weapons { get; set; } = new List<WeaponEntity>();
        public int MaxWeaponsCapacity { get; set; }
    }
}
