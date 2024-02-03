using AviationSalon.Core.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AviationSalon.Core.Abstractions.Services
{
    public interface IOrderItemService
    {
        Task<List<OrderItemEntity>> GetOrderItemsByOrderIdAsync(string orderId);
    }
}

