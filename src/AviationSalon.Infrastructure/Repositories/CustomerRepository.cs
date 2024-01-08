using AviationSalon.Core.Abstractions.Repositories;
using AviationSalon.Core.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AviationSalon.Infrastructure.Repositories
{
    public class CustomerRepository : IRepository<CustomerEntity>
    {
        public Task AddAsync(CustomerEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(CustomerEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<CustomerEntity>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<CustomerEntity> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(CustomerEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
