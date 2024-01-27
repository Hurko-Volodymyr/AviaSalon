using AviationSalon.Core.Data.Enums;

namespace AviationSalon.Core.Data.Entities
{
    public class WeaponEntity
    {
        public int WeaponId { get; set; }
        public string Name { get; set; }
        public WeaponType Type { get; set; }
        public GuidedSystemType GuidedSystem { get; set; }
        public int Range { get; set; }
        public int FirePower { get; set; }
        public AircraftEntity? Aircraft { get; set; }
        public int? AircraftId { get; set; }
    }
}