using AviationSalon.Core.Data.Entities;

namespace AviationSalon.WebUI.Models
{
    public class AircraftViewModel
    {
        public string Model { get; set; }
        public int Quantity { get; set; }
        public List<WeaponEntity> Weapons { get; set; }
    }

}
