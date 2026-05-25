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
    public class CartRepository : ICartRepository
    {
        private readonly MatrixIncDbContext _context;

        public CartRepository(MatrixIncDbContext context)
        {
            _context = context;
        }

        public IEnumerable<CartItem> GetCartItemsByCustomerId(int customerId)
        {
            return _context.CartItems
                .Where(cartItem => cartItem.CustomerId == customerId)
                .Include(cartItem => cartItem.Product)
                .ToList();
        }

        public void AddToCart(int customerId, int productId, int amount)
        {
            if (amount < 1)
            {
                amount = 1;
            }

            var existingCartItem = _context.CartItems
                .FirstOrDefault(cartItem =>
                    cartItem.CustomerId == customerId &&
                    cartItem.ProductId == productId);

            if (existingCartItem != null)
            {
                existingCartItem.Amount += amount;
                _context.CartItems.Update(existingCartItem);
            }
            else
            {
                var cartItem = new CartItem
                {
                    CustomerId = customerId,
                    ProductId = productId,
                    Amount = amount
                };

                _context.CartItems.Add(cartItem);
            }

            _context.SaveChanges();
        }

        public void RemoveFromCart(int customerId, int productId)
        {
            var cartItem = _context.CartItems
                .FirstOrDefault(cartItem =>
                    cartItem.CustomerId == customerId &&
                    cartItem.ProductId == productId);

            if (cartItem == null)
            {
                return;
            }

            _context.CartItems.Remove(cartItem);
            _context.SaveChanges();
        }

        public void ClearCart(int customerId)
        {
            var cartItems = _context.CartItems
                .Where(cartItem => cartItem.CustomerId == customerId);

            _context.CartItems.RemoveRange(cartItems);
            _context.SaveChanges();
        }
    }
}
