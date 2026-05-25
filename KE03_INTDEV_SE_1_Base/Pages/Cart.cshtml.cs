using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KE03_INTDEV_SE_1_Base.Pages
{
    public class CartModel : PageModel
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICustomerRepository _customerRepository;

        public CartModel(
            ICartRepository cartRepository,
            ICustomerRepository customerRepository)
        {
            _cartRepository = cartRepository;
            _customerRepository = customerRepository;
        }

        public IEnumerable<CartItem> CartItems { get; set; } = new List<CartItem>();

        public Customer? SelectedCustomer { get; set; }

        public decimal TotalPrice { get; set; }

        public IActionResult OnGet()
        {
            int? customerId = HttpContext.Session.GetInt32("CustomerId");

            if (!customerId.HasValue)
            {
                return RedirectToPage("/Account");
            }

            SelectedCustomer = _customerRepository.GetCustomerById(customerId.Value);

            CartItems = _cartRepository.GetCartItemsByCustomerId(customerId.Value);

            TotalPrice = CartItems.Sum(cartItem => cartItem.Product.Price * cartItem.Amount);

            return Page();
        }

        public IActionResult OnPostRemoveFromCart(int productId)
        {
            int? customerId = HttpContext.Session.GetInt32("CustomerId");

            if (!customerId.HasValue)
            {
                return RedirectToPage("/Account");
            }

            _cartRepository.RemoveFromCart(customerId.Value, productId);

            return RedirectToPage("/Cart");
        }
    }
}
