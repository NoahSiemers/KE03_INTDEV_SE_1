using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KE03_INTDEV_SE_1_Base.Pages.Products
{
    public class DetailsModel : PageModel
    {
        private readonly IProductRepository _productRepository;

        public DetailsModel(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public Product? Product { get; set; }

        [BindProperty]
        public int Amount { get; set; } = 1;

        public IActionResult OnGet(int id)
        {
            Product = _productRepository.GetProductById(id);

            if (Product == null)
            {
                return NotFound();
            }

            return Page();
        }

        public IActionResult OnPostOrder(int id)
        {
            return RedirectToPage("/Products/Details", new { id });
        }
    }
}