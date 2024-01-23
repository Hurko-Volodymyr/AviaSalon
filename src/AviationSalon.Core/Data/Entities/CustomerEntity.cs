namespace AviationSalon.Core.Data.Entities
{
    public class CustomerEntity
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string ContactInformation { get; set; }
        public List<OrderEntity> Orders { get; set; } = new List<OrderEntity>();
    }
}
