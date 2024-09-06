namespace Bakery.Models.DTO
{
    public class ChangeOrderRequest
    {
        public long OrderId { get; set; }
        public List<OrderItemDto> OrderItems {  get; set; }
    }
}
