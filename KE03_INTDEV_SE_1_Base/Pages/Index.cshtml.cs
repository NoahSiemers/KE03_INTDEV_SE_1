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

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Category { get; set; }

        [BindProperty(SupportsGet = true)]
        public decimal? MinPrice { get; set; }

        [BindProperty(SupportsGet = true)]
        public decimal? MaxPrice { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? PriceSort { get; set; }

        public void OnGet()
        {
            var products = _productRepository.GetAllProducts().ToList();

            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                string search = SearchTerm.ToLower();

                products = products
                    .Where(product =>
                        product.Name.ToLower().Contains(search) ||
                        product.Description.ToLower().Contains(search) ||
                        product.Category.ToLower().Contains(search))
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(Category))
            {
                products = products
                    .Where(product => product.Category == Category)
                    .ToList();
            }

            if (MinPrice.HasValue)
            {
                products = products
                    .Where(product => product.Price >= MinPrice.Value)
                    .ToList();
            }

            if (MaxPrice.HasValue)
            {
                products = products
                    .Where(product => product.Price <= MaxPrice.Value)
                    .ToList();
            }

            if (PriceSort == "price-low-high")
            {
                products = products
                    .OrderBy(product => product.Price)
                    .ToList();
            }
            else if (PriceSort == "price-high-low")
            {
                products = products
                    .OrderByDescending(product => product.Price)
                    .ToList();
            }
            else
            {
                products = products
                    .OrderBy(product => product.Name)
                    .ToList();
            }

            Products = products;

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

            return RedirectToPage("/Index", new
            {
                searchTerm = SearchTerm,
                category = Category,
                minPrice = MinPrice,
                maxPrice = MaxPrice
            });
        }
    }
}