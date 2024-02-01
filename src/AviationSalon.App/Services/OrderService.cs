using AviationSalon.Core.Abstractions.Repositories;
using AviationSalon.Core.Abstractions.Services;
using AviationSalon.Core.Data.Entities;
using AviationSalon.Core.Data.Enums;
using Microsoft.Extensions.Logging;

namespace AviationSalon.App.Services
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<OrderEntity> _orderRepository;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IRepository<OrderEntity> orderRepository, ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }

        public async Task PlaceOrderAsync(OrderEntity order)
        {
            try
            {
                order.OrderDate = DateTime.UtcNow;
                order.Status = OrderStatus.Pending;
                await _orderRepository.AddAsync(order);

                _logger.LogInformation($"Order placed successfully. OrderId: {order.OrderId}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error placing order. OrderId: {order.OrderId}. Error: {ex.Message}");
                throw; 
            }
        }

        public async Task<OrderEntity> GetOrderDetailsAsync(string orderId)
        {
            try
            {
                var order = await _orderRepository.GetByIdAsync(orderId);

                if (order != null)
                {
                    _logger.LogInformation($"Order details retrieved successfully. OrderId: {orderId}");
                }
                else
                {
                    _logger.LogWarning($"Order details not found. OrderId: {orderId}");
                }

                return order;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting order details. OrderId: {orderId}. Error: {ex.Message}");
                throw; 
            }
        }

        public async Task<List<OrderEntity>> GetCustomerOrdersAsync(string customerId)
        {
            try
            {
                var allOrders = await _orderRepository.GetAllAsync();
                var customerOrders = allOrders.Where(order => order.CustomerId == customerId).ToList();

                _logger.LogInformation($"Retrieved {customerOrders.Count} orders for customer. CustomerId: {customerId}");

                return customerOrders;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting customer orders. CustomerId: {customerId}. Error: {ex.Message}");
                throw;
            }
        }
    }

}
