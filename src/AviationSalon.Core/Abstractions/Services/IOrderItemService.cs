using AviationSalon.Core.Data.Entities;

namespace AviationSalon.Core.Abstractions.Services
{
    public interface IOrderItemService
    {
        Task<bool> TryCreateOrderItemAsync(string orderId, string aircraftId);
        Task<List<OrderItemEntity>> GetOrderItemsByOrderIdAsync(string orderId);
    }
}

