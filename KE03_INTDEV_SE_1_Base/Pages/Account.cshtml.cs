using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KE03_INTDEV_SE_1_Base.Pages
{
    public class AccountModel : PageModel
    {
        private readonly ICustomerRepository _customerRepository;

        public AccountModel(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public IEnumerable<Customer> Customers { get; set; } = new List<Customer>();

        public int? SelectedCustomerId { get; set; }

        public void OnGet()
        {
            Customers = _customerRepository.GetAllCustomers();
            SelectedCustomerId = HttpContext.Session.GetInt32("CustomerId");
        }

        public IActionResult OnPostSelectCustomer(int customerId)
        {
            HttpContext.Session.SetInt32("CustomerId", customerId);

            return RedirectToPage("/Account");
        }
    }
}
