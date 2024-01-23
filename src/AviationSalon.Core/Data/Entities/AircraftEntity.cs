namespace AviationSalon.Core.Data.Entities
{
    public class AircraftEntity
    {
        public int AircraftId { get; set; }
        public string Model { get; set; } = string.Empty;
        public string Manufacturer { get; set; } = string.Empty;
        public int YearOfManufacture { get; set; }
        public List<WeaponEntity> Weapons { get; set; } = new List<WeaponEntity>();
        public int MaxWeaponsCapacity { get; set; }
    }
}
