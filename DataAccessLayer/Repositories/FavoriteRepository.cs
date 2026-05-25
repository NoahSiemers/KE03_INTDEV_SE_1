using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly MatrixIncDbContext _context;

        public FavoriteRepository(MatrixIncDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Product> GetFavoritesByCustomerId(int customerId)
        {
            return _context.FavoriteProducts
                .Where(f => f.CustomerId == customerId)
                .Include(f => f.Product)
                    .ThenInclude(p => p.Images)
                .Include(f => f.Product)
                    .ThenInclude(p => p.Specifications)
                .Select(f => f.Product)
                .ToList();
        }

        public bool IsFavorite(int customerId, int productId)
        {
            return _context.FavoriteProducts
                .Any(f => f.CustomerId == customerId && f.ProductId == productId);
        }

        public void AddFavorite(int customerId, int productId)
        {
            bool alreadyExists = IsFavorite(customerId, productId);

            if (alreadyExists)
            {
                return;
            }

            var favorite = new FavoriteProduct
            {
                CustomerId = customerId,
                ProductId = productId
            };

            _context.FavoriteProducts.Add(favorite);
            _context.SaveChanges();
        }

        public void RemoveFavorite(int customerId, int productId)
        {
            var favorite = _context.FavoriteProducts
                .FirstOrDefault(f => f.CustomerId == customerId && f.ProductId == productId);

            if (favorite == null)
            {
                return;
            }

            _context.FavoriteProducts.Remove(favorite);
            _context.SaveChanges();
        }
    }
}
