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
        private readonly IRepository<AircraftEntity> _aircraftRepository;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IRepository<OrderEntity> orderRepository, IRepository<AircraftEntity> aircraftRepository, ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _aircraftRepository = aircraftRepository;
            _logger = logger;
        }

        public async Task<string> PlaceOrderAsync(List<string> aircraftIds, string customerId)
        {
            try
            {
                var order = new OrderEntity
                {
                    OrderId = Guid.NewGuid().ToString(),
                    OrderDate = DateTime.UtcNow,
                    Status = OrderStatus.Pending,
                    CustomerId = customerId,
                    OrderItems = new List<OrderItemEntity>(),
                    TotalQuantity = aircraftIds.Count,
                };

                foreach (var aircraftId in aircraftIds)
                {
                    var orderItem = new OrderItemEntity
                    {
                        OrderItemId = Guid.NewGuid().ToString(),
                        AircraftId = aircraftId,
                        OrderId = order.OrderId,
                        Quantity = 1,
                    };

                    order.OrderItems.Add(orderItem);
                }

                await _orderRepository.AddAsync(order);

                _logger.LogInformation($"Number of OrderItems in the order: {order.OrderItems.Count}");
                _logger.LogInformation($"Order placed successfully. OrderId: {order.OrderId}");
                return order.OrderId;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error placing order. Error: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> TryEditOrderAsync(string orderId, string selectedAircraftId)
        {
            try
            {
                var existingOrder = await _orderRepository.GetByIdAsync(orderId);

                if (existingOrder == null)
                {
                    _logger.LogWarning($"Order not found with ID: {orderId}");
                    return false;
                }

                var aircraft = await _aircraftRepository.GetByIdAsync(selectedAircraftId);

                if (aircraft == null)
                {
                    _logger.LogWarning($"Aircraft not found with ID: {selectedAircraftId}");
                    return false;
                }

                var existingOrderItem = existingOrder.OrderItems.FirstOrDefault(item => item.AircraftId == selectedAircraftId);

                if (existingOrderItem != null)
                {
                    existingOrderItem.Quantity++;
                    _logger.LogInformation($"Incrementing quantity for existing order item with aircraft ID: {selectedAircraftId}");
                }
                else
                {
                    var newOrderItem = new OrderItemEntity
                    {
                        OrderItemId = Guid.NewGuid().ToString(),
                        AircraftId = selectedAircraftId,
                        Aircraft = aircraft,
                        Quantity = 1,
                        OrderId = orderId,
                        Order = existingOrder,
                    };

                    existingOrder.OrderItems.Add(newOrderItem);
                    _logger.LogInformation($"Adding new order item with aircraft ID: {selectedAircraftId}");
                }

                existingOrder.TotalQuantity = existingOrder.OrderItems.Sum(item => item.Quantity);

                await _orderRepository.UpdateAsync(existingOrder);

                _logger.LogInformation($"Order updated successfully. Total quantity: {existingOrder.TotalQuantity}");

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while updating order. Exception: {ex.Message}");
                return false;
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
                var customerOrders = allOrders
                    .Where(order => order.CustomerId == customerId)
                    .OrderByDescending(order => order.OrderDate)
                    .ToList();

                _logger.LogInformation($"Retrieved {customerOrders.Count} orders for customer. CustomerId: {customerId}");

                return customerOrders;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting customer orders. CustomerId: {customerId}. Error: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> TryDeleteOrderAsync(string orderId)
        {
            try
            {
                var order = await _orderRepository.GetByIdAsync(orderId);

                if (order != null)
                {
                  
                    await _orderRepository.DeleteAsync(order);

                    _logger.LogInformation($"Order with ID {orderId} canceled successfully.");

                    return true;
                }

                _logger.LogWarning($"Order with ID {orderId} not found.");
                return false; 
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while canceling the order with ID {orderId}. Exception: {ex.Message}");
                return false;
            }
        }
    }

}
