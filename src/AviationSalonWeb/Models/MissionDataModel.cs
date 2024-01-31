using AviationSalon.Core.Data.Entities;

namespace AviationSalon.WebUI.Models
{
    public class MissionDataModel
    {
        public List<AircraftEntity> SelectedAircraft { get; set; } = new List<AircraftEntity>();
        public List<WeaponEntity> SelectedWeapons { get; set; } = new List<WeaponEntity>();
        public int CustomerId { get; set; }
    }
}