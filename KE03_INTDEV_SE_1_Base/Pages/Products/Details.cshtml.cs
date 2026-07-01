using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KE03_INTDEV_SE_1_Base.Pages.Products
{
    public class DetailsModel : PageModel
    {
        private readonly IProductRepository _productRepository;
        private readonly ICartRepository _cartRepository;

        public DetailsModel(IProductRepository productRepository, ICartRepository cartRepository)
        {
            _productRepository = productRepository;
            _cartRepository = cartRepository;
        }

        public Product? Product { get; set; }

        [BindProperty]
        public int Amount { get; set; } = 1;

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
                CartItems = _cartRepository.GetCartItemsByCustomerId(customerId.Value);
                CartTotalPrice = CartItems.Sum(cartItem => cartItem.Product.Price * cartItem.Amount);
            }

            return Page();
        }

        public IActionResult OnPostOrder(int id)
        {
            int? customerId = HttpContext.Session.GetInt32("CustomerId");

            if (!customerId.HasValue)
            {
                return RedirectToPage("/Account");
            }

            _cartRepository.AddToCart(customerId.Value, id, Amount);

            TempData["CartMessage"] = "Succesvol aan winkelwagen toegevoegd";

            return RedirectToPage("/Products/Details", new { id });
        }
    }
}