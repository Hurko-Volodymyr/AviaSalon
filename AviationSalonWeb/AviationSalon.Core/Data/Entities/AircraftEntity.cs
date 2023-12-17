using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AviationSalon.Core.Data.Entities
{
    public class AircraftEntity
    {
        public int AircraftId { get; set; }
        public string Model { get; set; }
        public string Manufacturer { get; set; }
        public int YearOfManufacture { get; set; }
        public decimal Price { get; set; }
    }
}
