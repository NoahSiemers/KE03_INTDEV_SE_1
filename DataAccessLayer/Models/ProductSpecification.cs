namespace DataAccessLayer.Models
{
    public class ProductSpecification
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public string SpecName { get; set; } = string.Empty;

        public string SpecValue { get; set; } = string.Empty;

        public Product? Product { get; set; }
    }
}
