using AviationSalon.Core.Abstractions.Repositories;
using AviationSalon.Core.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AviationSalon.Infrastructure.Repositories
{
    public class OrderRepository : IRepository<OrderEntity>
    {
        private readonly ApplicationDbContext _dbContext;

        public OrderRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new();
        }
        public async Task AddAsync(OrderEntity entity)
        {
            _dbContext.Orders.Add(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(OrderEntity entity)
        {
            _dbContext.Orders.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<OrderEntity>> GetAllAsync()
        {
            return await _dbContext.Orders.ToListAsync();
        }

        public async Task<OrderEntity> GetByIdAsync(int id)
        {
            return await _dbContext.Orders.FindAsync(id); 
        }

        public async Task UpdateAsync(OrderEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
