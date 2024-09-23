namespace Infinion.Core.DTOs
{
    public class ProductDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string? PhotoURL { get; set; }
        public string Description { get; set; } 
        public string Brand { get; set; } 
        public double Price { get; set; }
        public string AddedBy { get; set; }
    }
}
