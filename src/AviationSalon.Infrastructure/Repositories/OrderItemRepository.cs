using AviationSalon.Core.Abstractions.Repositories;
using AviationSalon.Core.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AviationSalon.Infrastructure.Repositories
{
    public class OrderItemRepository : IRepository<OrderItemEntity>
    {
        public Task AddAsync(OrderItemEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(OrderItemEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<OrderItemEntity>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<OrderItemEntity> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(OrderItemEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
