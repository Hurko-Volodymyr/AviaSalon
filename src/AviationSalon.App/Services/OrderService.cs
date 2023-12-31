﻿using AviationSalon.Core.Abstractions.Repositories;
using AviationSalon.Core.Abstractions.Services;
using AviationSalon.Core.Data.Entities;
using AviationSalon.Core.Data.Enums;

namespace AviationSalon.App.Services
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<OrderEntity> _orderRepository;

        public OrderService(IRepository<OrderEntity> orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task PlaceOrderAsync(OrderEntity order)
        {
            order.OrderDate = DateTime.UtcNow;
            order.Status = OrderStatus.Pending;
            await _orderRepository.AddAsync(order);
        }

        public async Task<OrderEntity> GetOrderDetailsAsync(int orderId)
        {
            return await _orderRepository.GetByIdAsync(orderId);
        }

        public async Task<List<OrderEntity>> GetCustomerOrdersAsync(int customerId)
        {
            return await _orderRepository.GetAllAsync();
        }
    }
}
