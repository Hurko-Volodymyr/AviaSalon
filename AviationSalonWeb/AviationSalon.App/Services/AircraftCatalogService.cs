using AviationSalon.Core.Abstractions.Reositories;
using AviationSalon.Core.Abstractions.Services;
using AviationSalon.Core.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AviationSalon.App.Services
{
    public class AircraftCatalogService : IAircraftCatalogService
    {
        private readonly IRepository<AircraftEntity> _aircraftRepository;

        public AircraftCatalogService(IRepository<AircraftEntity> aircraftRepository)
        {
            _aircraftRepository = aircraftRepository;
        }

        public async Task<List<AircraftEntity>> GetAircraftListAsync()
        {
            var aircraftEntities = await _aircraftRepository.GetAllAsync();
            return aircraftEntities.ToList();
        }

        public async Task<AircraftEntity> GetAircraftDetailsAsync(int aircraftId)
        {
            var aircraftEntity = await _aircraftRepository.GetByIdAsync(aircraftId);
            return aircraftEntity;
        }
    }
}
