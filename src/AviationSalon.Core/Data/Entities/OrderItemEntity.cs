namespace AviationSalon.Core.Data.Entities
{
    public class OrderItemEntity
    {
        public string OrderItemId { get; set; }
        public string OrderId { get; set; }
        public OrderEntity Order { get; set; }
        public string AircraftId { get; set; }
        public AircraftEntity Aircraft { get; set; }
        public int Quantity { get; set; }
    }
}
