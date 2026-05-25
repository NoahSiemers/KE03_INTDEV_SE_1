using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class FavoriteProduct
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public Customer Customer { get; set; } = null!;

        public int ProductId { get; set; }

        public Product Product { get; set; } = null!;
    }
}
