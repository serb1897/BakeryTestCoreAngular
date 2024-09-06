namespace Bakery.Models.DbModels
{
    public class OrderStatus
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
