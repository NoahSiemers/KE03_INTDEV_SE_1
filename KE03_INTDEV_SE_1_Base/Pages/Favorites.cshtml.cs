using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KE03_INTDEV_SE_1_Base.Pages
{
    public class FavoritesModel : PageModel
    {
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly ICustomerRepository _customerRepository;

        public FavoritesModel(
            IFavoriteRepository favoriteRepository,
            ICustomerRepository customerRepository)
        {
            _favoriteRepository = favoriteRepository;
            _customerRepository = customerRepository;
        }

        public IEnumerable<Product> FavoriteProducts { get; set; } = new List<Product>();

        public Customer? SelectedCustomer { get; set; }

        public IActionResult OnGet()
        {
            int? customerId = HttpContext.Session.GetInt32("CustomerId");

            if (!customerId.HasValue)
            {
                return RedirectToPage("/Account");
            }

            SelectedCustomer = _customerRepository.GetCustomerById(customerId.Value);

            FavoriteProducts = _favoriteRepository.GetFavoritesByCustomerId(customerId.Value);

            return Page();
        }

        public IActionResult OnPostRemoveFavorite(int productId)
        {
            int? customerId = HttpContext.Session.GetInt32("CustomerId");

            if (!customerId.HasValue)
            {
                return RedirectToPage("/Account");
            }

            _favoriteRepository.RemoveFavorite(customerId.Value, productId);

            return RedirectToPage("/Favorites");
        }
    }
}
