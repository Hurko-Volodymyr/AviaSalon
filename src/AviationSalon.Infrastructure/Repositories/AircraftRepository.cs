using AviationSalon.Core.Abstractions.Repositories;
using AviationSalon.Core.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AviationSalon.Infrastructure.Repositories
{
    public class AircraftRepository : IRepository<AircraftEntity>
    {
        private readonly ApplicationDbContext _dbContext;

        public AircraftRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new();
        }

        public async Task<List<AircraftEntity>> GetAllAsync()
        {
            return await _dbContext.Aircrafts.ToListAsync();
        }

        public async Task<AircraftEntity> GetByIdAsync(string id)
        {
            return await _dbContext.Aircrafts.FindAsync(id);
        }

        public async Task AddAsync(AircraftEntity entity)
        {
            if (entity == null)
            {
                throw new();
            }

            await _dbContext.Aircrafts.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(AircraftEntity entity)
        {
            if (entity == null)
            {
                throw new();
            }

            _dbContext.Aircrafts.Update(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(AircraftEntity entity)
        {
            if (entity == null)
            {
                throw new();
            }

            _dbContext.Aircrafts.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
    }

}
