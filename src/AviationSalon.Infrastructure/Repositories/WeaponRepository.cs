using AviationSalon.Core.Abstractions.Repositories;
using AviationSalon.Core.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AviationSalon.Infrastructure.Repositories
{
    public class WeaponRepository : IRepository<WeaponEntity>
    {
        private readonly ApplicationDbContext _dbContext;

        public WeaponRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new();
        }
        public async Task<List<WeaponEntity>> GetAllAsync()
        {
            return await _dbContext.Weapons.ToListAsync();
        }

        public async Task<WeaponEntity> GetByIdAsync(string id)
        {
            return await _dbContext.Weapons
                .Include(w => w.Aircraft) 
                .FirstOrDefaultAsync(w => w.WeaponId == id);
        }


        public async Task AddAsync(WeaponEntity entity)
        {
            _dbContext.Weapons.Add(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(WeaponEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(WeaponEntity entity)
        {
            _dbContext.Weapons.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
    }


}
