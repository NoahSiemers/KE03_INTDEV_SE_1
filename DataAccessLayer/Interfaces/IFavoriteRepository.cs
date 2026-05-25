using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface IFavoriteRepository
    {
        IEnumerable<Product> GetFavoritesByCustomerId(int customerId);

        bool IsFavorite(int customerId, int productId);

        void AddFavorite(int customerId, int productId);

        void RemoveFavorite(int customerId, int productId);
    }

}
