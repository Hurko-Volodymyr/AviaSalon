﻿using AviationSalon.Core.Data.Enums;

namespace AviationSalon.Core.Data.Entities
{
    public class OrderEntity
    {
        public string OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerId { get; set; }
        public CustomerEntity Customer { get; set; }
        public List<OrderItemEntity> OrderItems { get; set; } = new List<OrderItemEntity>();
        public decimal TotalQuantity { get; set; }
        public OrderStatus Status { get; set; }
    }
}

