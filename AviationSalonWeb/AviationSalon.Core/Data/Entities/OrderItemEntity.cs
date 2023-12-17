using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AviationSalon.Core.Data.Entities
{
    public class OrderItemEntity
    {   public int OrderItemId { get; set; }
        public int AircraftId { get; set; }
        public AircraftEntity Aircraft { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
