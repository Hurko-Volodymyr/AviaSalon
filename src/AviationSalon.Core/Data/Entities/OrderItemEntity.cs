namespace AviationSalon.Core.Data.Entities
{
    public class OrderItemEntity
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public OrderEntity Order { get; set; }
        public int AircraftId { get; set; }
        public AircraftEntity Aircraft { get; set; }
        public int Quantity { get; set; }
    }
}
