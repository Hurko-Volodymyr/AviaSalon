namespace AviationSalon.Core.Data.Entities
{
    public class AircraftEntity
    {
        public int AircraftId { get; set; }
        public string Model { get; set; }
        public string Manufacturer { get; set; }
        public int YearOfManufacture { get; set; }
        public List<WeaponEntity> Weapons { get; set; } = new List<WeaponEntity>();
        public int MaxWeaponsCapacity { get; set; }
    }
}
