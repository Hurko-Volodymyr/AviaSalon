using AviationSalon.Core.Data.Entities;

namespace AviationSalon.Core.Abstractions.Services
{
    public interface ICustomerService
    {
        Task<bool> AddOrderToCustomerAsync(string userSecret, string orderId);
        Task<CustomerEntity> GetCustomerDetailsAsync(string customerId);
        Task UpdateCustomerDetailsAsync(string customerId, string name, string contactInformation);

    }
}
