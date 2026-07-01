using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public string? MainImageUrl { get; set; }

        public string Category { get; set; } = string.Empty;

        public ICollection<Order> Orders { get; } = new List<Order>();

        public ICollection<Part> Parts { get; } = new List<Part>();

        public ICollection<ProductImage> Images { get; } = new List<ProductImage>();

        public ICollection<ProductSpecification> Specifications { get; } = new List<ProductSpecification>();
    }
}
