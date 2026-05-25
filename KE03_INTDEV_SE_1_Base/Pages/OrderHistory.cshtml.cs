using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KE03_INTDEV_SE_1_Base.Pages
{
    public class OrderHistoryModel : PageModel
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;

        public OrderHistoryModel(
            IOrderRepository orderRepository,
            ICustomerRepository customerRepository)
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
        }

        public IEnumerable<Order> Orders { get; set; } = new List<Order>();

        public Customer? SelectedCustomer { get; set; }

        public IActionResult OnGet()
        {
            int? customerId = HttpContext.Session.GetInt32("CustomerId");

            if (!customerId.HasValue)
            {
                return RedirectToPage("/Account");
            }

            SelectedCustomer = _customerRepository.GetCustomerById(customerId.Value);

            Orders = _orderRepository.GetOrdersByCustomerId(customerId.Value);

            return Page();
        }
    }
}
