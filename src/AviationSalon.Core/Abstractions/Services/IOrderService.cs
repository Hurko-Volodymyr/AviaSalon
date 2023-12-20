using AviationSalon.Core.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AviationSalon.Core.Abstractions.Services
{
    public interface IOrderService
    {
        Task PlaceOrderAsync(OrderEntity order);
        Task<OrderEntity> GetOrderDetailsAsync(int orderId);
        Task<List<OrderEntity>> GetCustomerOrdersAsync(int customerId);
        Task<decimal> CalculateOrderTotalAsync(OrderEntity order);
    }


}
