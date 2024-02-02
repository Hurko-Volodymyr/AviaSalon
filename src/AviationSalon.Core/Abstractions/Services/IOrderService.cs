using AviationSalon.Core.Data.Entities;

namespace AviationSalon.Core.Abstractions.Services
{
    public interface IOrderService
    {
        Task PlaceOrderAsync(List<string> selectedAircraftsId, string customerId);
        Task<OrderEntity> GetOrderDetailsAsync(string orderId);
        Task<List<OrderEntity>> GetCustomerOrdersAsync(string customerId);
    }


}
