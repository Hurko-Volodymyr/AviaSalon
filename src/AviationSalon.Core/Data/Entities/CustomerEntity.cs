using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AviationSalon.Core.Data.Entities
{
    public class CustomerEntity
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string ContactInformation { get; set; }
    }
}
