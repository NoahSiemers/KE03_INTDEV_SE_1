using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface ICartRepository
    {
        IEnumerable<CartItem> GetCartItemsByCustomerId(int customerId);

        void AddToCart(int customerId, int productId, int amount);

        void UpdateAmount(int customerId, int productId, int amount);

        void RemoveFromCart(int customerId, int productId);

        void ClearCart(int customerId);
    }
}
