namespace AviationSalon.WebUI.Models
{
    public class OrderModel
    {
        public List<OrderItemModel> OrderItems { get; set; } = new List<OrderItemModel>();
    }
}
