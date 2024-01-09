using AviationSalon.Core.Abstractions.Repositories;
using AviationSalon.Core.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AviationSalon.Infrastructure.Repositories
{
    public class OrderRepository : IRepository<OrderEntity>
    {
        public Task AddAsync(OrderEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(OrderEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<OrderEntity>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<OrderEntity> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(OrderEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
