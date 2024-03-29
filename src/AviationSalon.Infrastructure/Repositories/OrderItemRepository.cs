﻿using AviationSalon.Core.Abstractions.Repositories;
using AviationSalon.Core.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AviationSalon.Infrastructure.Repositories
{
    public class OrderItemRepository : IRepository<OrderItemEntity>
    {
        private readonly ApplicationDbContext _dbContext;

        public OrderItemRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new();
        }
        public async Task AddAsync(OrderItemEntity entity)
        {
            _dbContext.OrderItems.Add(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(OrderItemEntity entity)
        {
            _dbContext.OrderItems.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<OrderItemEntity>> GetAllAsync()
        {
            return await _dbContext.OrderItems.ToListAsync();
        }

        public async Task<OrderItemEntity> GetByIdAsync(string id)
        {
            return await _dbContext.OrderItems
                .Include(oi => oi.Order)
                    .ThenInclude(o => o.Customer)
                .Include(oi => oi.Aircraft)
                    .ThenInclude(a => a.Weapons)
                .FirstOrDefaultAsync(oi => oi.OrderItemId == id);
        }


        public async Task UpdateAsync(OrderItemEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
