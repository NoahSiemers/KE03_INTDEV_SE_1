using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KE03_INTDEV_SE_1_Base.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IProductRepository _productRepository;
        private readonly IFavoriteRepository _favoriteRepository;

        public IndexModel(
            IProductRepository productRepository,
            IFavoriteRepository favoriteRepository)
        {
            _productRepository = productRepository;
            _favoriteRepository = favoriteRepository;
        }

        public IEnumerable<Product> Products { get; set; } = new List<Product>();

        public List<int> FavoriteProductIds { get; set; } = new();

        public void OnGet()
        {
            Products = _productRepository.GetAllProducts();

            int? customerId = HttpContext.Session.GetInt32("CustomerId");

            if (customerId.HasValue)
            {
                FavoriteProductIds = _favoriteRepository
                    .GetFavoritesByCustomerId(customerId.Value)
                    .Select(product => product.Id)
                    .ToList();
            }
        }

        public IActionResult OnPostToggleFavorite(int productId)
        {
            int? customerId = HttpContext.Session.GetInt32("CustomerId");

            if (!customerId.HasValue)
            {
                return RedirectToPage("/Account");
            }

            bool isFavorite = _favoriteRepository.IsFavorite(customerId.Value, productId);

            if (isFavorite)
            {
                _favoriteRepository.RemoveFavorite(customerId.Value, productId);
            }
            else
            {
                _favoriteRepository.AddFavorite(customerId.Value, productId);
            }

            return RedirectToPage("/Index");
        }
    }
}