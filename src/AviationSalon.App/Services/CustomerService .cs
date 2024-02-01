using AviationSalon.Core.Abstractions.Repositories;
using AviationSalon.Core.Abstractions.Services;
using AviationSalon.Core.Data.Entities;

namespace AviationSalon.App.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IRepository<CustomerEntity> _customerRepository;

        public CustomerService(IRepository<CustomerEntity> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<CustomerEntity> GetCustomerDetailsAsync(string customerId)
        {
            return await _customerRepository.GetByIdAsync(customerId);
        }

        public async Task UpdateCustomerDetailsAsync(string customerId, CustomerEntity updatedCustomer)
        {
            var existingCustomer = await _customerRepository.GetByIdAsync(customerId);

            if (existingCustomer != null)
            {
                existingCustomer.Name = updatedCustomer.Name;
                existingCustomer.ContactInformation = updatedCustomer.ContactInformation;

                await _customerRepository.UpdateAsync(existingCustomer);
            }
        }
    }
}
