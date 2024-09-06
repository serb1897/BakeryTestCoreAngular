namespace Bakery.Models.DbModels
{
    public class Customer
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string DeliveryAddress { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
