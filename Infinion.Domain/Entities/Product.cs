namespace Infinion.Domain.Entities
{
    public class Product:Entity
    {
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string? PhotoURL { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public double Price { get; set; }
        public string AddedBy { get; set; }
    }
}

