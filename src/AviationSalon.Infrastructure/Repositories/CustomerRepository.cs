using AviationSalon.Core.Abstractions.Repositories;
using AviationSalon.Core.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AviationSalon.Infrastructure.Repositories
{
    public class CustomerRepository : IRepository<CustomerEntity>
    {
        private readonly ApplicationDbContext _dbContext;

        public CustomerRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new();
        }
        public async Task AddAsync(CustomerEntity entity)
        {
            _dbContext.Customers.Add(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(CustomerEntity entity)
        {
            _dbContext.Customers.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<CustomerEntity>> GetAllAsync()
        {
            return await _dbContext.Customers.ToListAsync();
        }

        public async Task<CustomerEntity> GetByIdAsync(string id)
        {
            return await _dbContext.Customers
                .Include(c => c.Orders)
                    .ThenInclude(o => o.OrderItems)
                .FirstOrDefaultAsync(c => c.CustomerId == id);
        }


        public async Task UpdateAsync(CustomerEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
