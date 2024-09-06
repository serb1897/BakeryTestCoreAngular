namespace Bakery.Models.DTO
{
    public class CreateOrderRequest
    {
        public long? Id { get; set; }
        public DateTime? OrderDate { get; set; }
        public decimal? TotalCost { get; set; }
        public long CustomerId { get; set; }
        public long StatusId { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
    }
}
