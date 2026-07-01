using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using KE03_INTDEV_SE_1_Base.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KE03_INTDEV_SE_1_Base.Pages
{
    public class AccountModel : PageModel
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ICartRepository _cartRepository;

        public AccountModel(
            ICustomerRepository customerRepository,
            ICartRepository cartRepository)
        {
            _customerRepository = customerRepository;
            _cartRepository = cartRepository;
        }

        [BindProperty(SupportsGet = true)]
        public string? ReturnUrl { get; set; }

        public IEnumerable<Customer> Customers { get; set; } = new List<Customer>();

        public int? SelectedCustomerId { get; set; }

        public void OnGet()
        {
            Customers = _customerRepository.GetAllCustomers();
            SelectedCustomerId = HttpContext.Session.GetInt32("CustomerId");
        }

        public IActionResult OnPostSelectCustomer(int customerId, string? returnUrl)
        {
            HttpContext.Session.SetInt32("CustomerId", customerId);

            var guestCartItems = CartSessionHelper.GetGuestCart(HttpContext.Session);

            bool hadGuestCartItems = guestCartItems.Any();

            foreach (var guestItem in guestCartItems)
            {
                _cartRepository.AddToCart(customerId, guestItem.ProductId, guestItem.Amount);
            }

            CartSessionHelper.ClearGuestCart(HttpContext.Session);

            if (hadGuestCartItems && returnUrl == "/Checkout")
            {
                TempData["CartMergedMessage"] = "Je tijdelijke winkelwagen is toegevoegd aan de winkelwagen van dit account. Controleer je winkelwagen voordat je verdergaat naar de kassa.";

                return RedirectToPage("/Cart");
            }

            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }

            return RedirectToPage("/Account");
        }
    }
}
