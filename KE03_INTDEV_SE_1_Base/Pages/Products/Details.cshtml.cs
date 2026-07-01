using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using KE03_INTDEV_SE_1_Base.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KE03_INTDEV_SE_1_Base.Pages.Products
{
    public class DetailsModel : PageModel
    {
        private readonly IProductRepository _productRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IFavoriteRepository _favoriteRepository;

        public DetailsModel(IProductRepository productRepository, ICartRepository cartRepository, IFavoriteRepository favoriteRepository)
        {
            _productRepository = productRepository;
            _cartRepository = cartRepository;
            _favoriteRepository = favoriteRepository;
        }

        public Product? Product { get; set; }

        public bool IsFavorite { get; set; }

        [BindProperty]
        public int Amount { get; set; } = 1;

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

        public IEnumerable<CartItem> CartItems { get; set; } = new List<CartItem>();
        public decimal CartTotalPrice { get; set; }

        public IActionResult OnGet(int id)
        {
            Product = _productRepository.GetProductById(id);

            if (Product == null)
            {
                return NotFound();
            }

            int? customerId = HttpContext.Session.GetInt32("CustomerId");

            if (customerId.HasValue)
            {
                IsFavorite = _favoriteRepository.IsFavorite(customerId.Value, id);

                CartItems = _cartRepository.GetCartItemsByCustomerId(customerId.Value).ToList();
            }
            else
            {
                var guestCartItems = CartSessionHelper.GetGuestCart(HttpContext.Session);
                var cartItems = new List<CartItem>();

                foreach (var guestItem in guestCartItems)
                {
                    var product = _productRepository.GetProductById(guestItem.ProductId);

                    if (product != null)
                    {
                        cartItems.Add(new CartItem
                        {
                            ProductId = guestItem.ProductId,
                            Product = product,
                            Amount = guestItem.Amount
                        });
                    }
                }

                CartItems = cartItems;
            }

            CartTotalPrice = CartItems.Sum(cartItem => cartItem.Product.Price * cartItem.Amount);

            return Page();
        }

        public IActionResult OnPostOrder(int id)
        {
            if (Amount < 1)
            {
                Amount = 1;
            }

            int? customerId = HttpContext.Session.GetInt32("CustomerId");

            if (customerId.HasValue)
            {
                _cartRepository.AddToCart(customerId.Value, id, Amount);
            }
            else
            {
                CartSessionHelper.AddToGuestCart(HttpContext.Session, id, Amount);
            }

            TempData["CartMessage"] = "Succesvol aan winkelwagen toegevoegd";

            return RedirectToPage("/Products/Details", new
            {
                id,
                searchTerm = SearchTerm,
                category = Category,
                minPrice = MinPrice,
                maxPrice = MaxPrice,
                priceSort = PriceSort
            });
        }

        public IActionResult OnPostToggleFavorite(int id)
        {
            int? customerId = HttpContext.Session.GetInt32("CustomerId");

            if (!customerId.HasValue)
            {
                return RedirectToPage("/Account");
            }

            bool isFavorite = _favoriteRepository.IsFavorite(customerId.Value, id);

            if (isFavorite)
            {
                _favoriteRepository.RemoveFavorite(customerId.Value, id);
            }
            else
            {
                _favoriteRepository.AddFavorite(customerId.Value, id);
            }

            return RedirectToPage("/Products/Details", new { id });
        }
    }
}