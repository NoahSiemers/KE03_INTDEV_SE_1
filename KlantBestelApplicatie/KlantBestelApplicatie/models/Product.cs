namespace KlantBestelApplicatie.models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public string? MainImageUrl { get; set; }

        public List<ProductImage> Images { get; set; } = new();

        public List<ProductSpecification> Specifications { get; set; } = new();
    }
}
