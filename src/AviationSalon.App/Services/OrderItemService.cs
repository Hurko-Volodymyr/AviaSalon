using AviationSalon.Core.Abstractions.Repositories;
using AviationSalon.Core.Abstractions.Services;
using AviationSalon.Core.Data.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AviationSalon.App.Services
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IRepository<OrderItemEntity> _orderItemRepository;
        private readonly IRepository<AircraftEntity> _aircraftRepository;
        private readonly ILogger<OrderItemService> _logger;
        public OrderItemService(IRepository<OrderItemEntity> orderItemRepository, IRepository<AircraftEntity> aircraftRepository, ILogger<OrderItemService> logger)
        {
            _orderItemRepository = orderItemRepository;
            _aircraftRepository = aircraftRepository;
            _logger = logger;
        }


        public async Task<bool> CreateOrderItemAsync(string orderId, string aircraftId)
        {
            try
            {
                var aircraft = await _aircraftRepository.GetByIdAsync(aircraftId);

                if (aircraft == null)
                {
                    _logger.LogWarning($"Aircraft not found with ID: {aircraftId}");
                    return false;
                }

                var newOrderItem = new OrderItemEntity
                {
                    OrderItemId = Guid.NewGuid().ToString(),
                    AircraftId = aircraftId,
                    Aircraft = aircraft,
                    Quantity = 1,
                    OrderId = orderId,
                };

                await _orderItemRepository.AddAsync(newOrderItem);

                _logger.LogInformation($"Order item created successfully with ID: {newOrderItem.OrderItemId}");

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while creating order item. Exception: {ex.Message}");
                return false;
            }
        }
        public async Task<List<OrderItemEntity>> GetOrderItemsByOrderIdAsync(string orderId)
        {
            try
            {
                var allOrderItems = await _orderItemRepository.GetAllAsync();
                var orderItems = allOrderItems.Where(item => item.OrderId == orderId).ToList();

                if (orderItems != null && orderItems.Any())
                {
                    _logger.LogInformation($"Order items retrieved successfully for OrderId: {orderId}");
                }
                else
                {
                    _logger.LogWarning($"Order items not found for OrderId: {orderId}");
                }

                return orderItems;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting order items. OrderId: {orderId}. Error: {ex.Message}");
                throw;
            }
        }
    }
}
