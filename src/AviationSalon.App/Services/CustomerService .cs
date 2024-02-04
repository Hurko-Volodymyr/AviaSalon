using AviationSalon.Core.Abstractions.Repositories;
using AviationSalon.Core.Abstractions.Services;
using AviationSalon.Core.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AviationSalon.App.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IRepository<CustomerEntity> _customerRepository;
        private readonly IRepository<OrderEntity> _orderRepository;
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(IRepository<CustomerEntity> customerRepository, IRepository<OrderEntity> orderRepository, ILogger<CustomerService> logger)
        {
            _customerRepository = customerRepository;
            _orderRepository = orderRepository;
            _logger = logger;
        }

        public async Task<CustomerEntity> GetCustomerDetailsAsync(string customerId)
        {
            return await _customerRepository.GetByIdAsync(customerId);
        }

        public async Task UpdateCustomerDetailsAsync(string customerId, string name, string contactInformation)
        {
            var existingCustomer = await _customerRepository.GetByIdAsync(customerId);

            if (existingCustomer != null)
            {
                existingCustomer.Name = name;
                existingCustomer.ContactInformation = contactInformation;

                await _customerRepository.UpdateAsync(existingCustomer);
            }
        }

        public async Task<bool> AddOrderToCustomerAsync(string userSecret, string orderId)
        {
            try
            {
                var customer = await _customerRepository.GetByIdAsync(userSecret);
                var order = await _orderRepository.GetByIdAsync(orderId);

                if (customer != null && order != null)
                {
                    customer.Orders.Add(order);                   

                    _logger.LogInformation($"Order {orderId} added to Customer ID: {customer.CustomerId}");
                    return true;
                }

                _logger.LogWarning($"Customer not found for User Secret: {userSecret}");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error adding order to customer: {ex.Message}");
                return false;
            }
        }
    }
}
