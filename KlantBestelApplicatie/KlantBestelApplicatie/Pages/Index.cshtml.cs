using KlantBestelApplicatie.Data;
using KlantBestelApplicatie.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace KlantBestelApplicatie.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Product> Products { get; set; } = new();

        public async Task OnGetAsync()
        {
            Products = await _context.Products
                .OrderBy(product => product.Name)
                .ToListAsync();
        }
    }
}
