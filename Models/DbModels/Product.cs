namespace Bakery.Models.DbModels
{
    public class Product
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public decimal Price { get; set; }
        public long CategoryId { get; set; }
        public bool IsAvailable { get; set; }

        public Category Category { get; set; }
    }
}
