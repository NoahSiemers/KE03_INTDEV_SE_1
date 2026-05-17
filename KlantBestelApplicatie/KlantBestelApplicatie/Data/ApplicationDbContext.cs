using KlantBestelApplicatie.models;
using Microsoft.EntityFrameworkCore;

namespace KlantBestelApplicatie.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<ProductImage> ProductImages => Set<ProductImage>();
        public DbSet<ProductSpecification> ProductSpecifications => Set<ProductSpecification>();
    }
}
