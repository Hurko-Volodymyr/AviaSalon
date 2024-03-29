﻿using AviationSalon.Core.Abstractions.Repositories;
using AviationSalon.Core.Data.Entities;
using Microsoft.EntityFrameworkCore;

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

        public async Task<OrderEntity> GetByIdAsync(string id)
        {
            return await _dbContext.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.OrderId == id);
        }


        public async Task UpdateAsync(OrderEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
