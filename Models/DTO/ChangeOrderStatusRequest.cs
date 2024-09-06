namespace Bakery.Models.DTO
{
    public class ChangeOrderStatusRequest
    {
        public long OrderId { get; set; }
        public long StatusId { get; set; }
    }
}
