namespace Bakery.Models.DTO
{
    public class ProductDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public long CategoryId { get; set; }
        public string CategoryName { get; set; }
        public bool IsAvailable { get; set; }
    }
}
