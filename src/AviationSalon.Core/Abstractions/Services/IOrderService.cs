using AviationSalon.Core.Data.Entities;

namespace AviationSalon.Core.Abstractions.Services
{
    public interface IOrderService
    {
        Task<string> PlaceOrderAsync(List<string> selectedAircraftsId, string customerId);
        Task<bool> EditOrderAsync(string orderId, string selectedAircraftId);
        Task<OrderEntity> GetOrderDetailsAsync(string orderId);
        Task<List<OrderEntity>> GetCustomerOrdersAsync(string customerId);
        Task<bool> DeleteOrderAsync(string orderId);
    }


}
