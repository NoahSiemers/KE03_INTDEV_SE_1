using KlantBestelApplicatie.Data;
using KlantBestelApplicatie.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace KlantBestelApplicatie.Pages.Products
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Product? Product { get; set; }

        [BindProperty]
        public int Amount { get; set; } = 1;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Product = await _context.Products
                .Include(product => product.Images)
                .Include(product => product.Specifications)
                .FirstOrDefaultAsync(product => product.Id == id);

            if (Product == null)
            {
                return NotFound();
            }

            return Page();
        }

        public IActionResult OnPostOrder(int id)
        {
            // Later kun je hier het product toevoegen aan je winkelwagen.
            // Voor nu sturen we tijdelijk terug naar dezelfde pagina.
            return RedirectToPage("/Products/Details", new { id });
        }
    }
}
