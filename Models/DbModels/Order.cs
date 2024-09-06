namespace Bakery.Models.DbModels
{
    public class Order
    {
        public long Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalCost { get; set; }
        public long StatusId { get; set; }
        public long CustomerId { get; set; }

        public OrderStatus Status { get; set; }
        public Customer Customer { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
