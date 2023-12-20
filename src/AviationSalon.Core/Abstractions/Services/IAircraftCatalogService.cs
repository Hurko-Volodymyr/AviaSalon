using AviationSalon.Core.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AviationSalon.Core.Abstractions.Services
{
    public interface IAircraftCatalogService
    {
        Task<List<AircraftEntity>> GetAircraftListAsync();
        Task<AircraftEntity> GetAircraftDetailsAsync(int aircraftId);
    }

}
