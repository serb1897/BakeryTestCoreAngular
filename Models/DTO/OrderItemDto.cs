namespace Bakery.Models.DTO
{
    public class OrderItemDto
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal ItemCost { get; set; }
        public long? OrderId { get; set; }
    }
}
