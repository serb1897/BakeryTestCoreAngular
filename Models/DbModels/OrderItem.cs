namespace Bakery.Models.DbModels
{
    public class OrderItem
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public long OrderId { get; set; }
        public int Quantity { get; set; }
        public decimal ItemCost { get; set; }

        public Product Product { get; set; }
        public Order Order { get; set; }
    }
}
