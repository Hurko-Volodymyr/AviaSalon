using AviationSalon.Core.Data.Entities;

namespace AviationSalon.Core.Abstractions.Services
{
    public interface ICustomerService
    {
        public interface ICustomerService
        {
            Task<CustomerEntity> GetCustomerDetailsAsync(int customerId);
            Task UpdateCustomerDetailsAsync(int customerId, CustomerEntity updatedCustomer);
        }

    }
}
