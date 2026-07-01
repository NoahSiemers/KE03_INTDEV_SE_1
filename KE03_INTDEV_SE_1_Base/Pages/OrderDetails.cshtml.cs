using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KE03_INTDEV_SE_1_Base.Pages
{
    public class OrderDetailsModel : PageModel
    {
        private readonly IOrderRepository _orderRepository;

        public OrderDetailsModel(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public Order? Order { get; set; }

        public IActionResult OnGet(int orderId)
        {
            int? customerId = HttpContext.Session.GetInt32("CustomerId");

            if (!customerId.HasValue)
            {
                return RedirectToPage("/Account");
            }

            Order = _orderRepository.GetOrderById(orderId);

            if (Order == null)
            {
                return NotFound();
            }

            if (Order.CustomerId != customerId.Value)
            {
                return RedirectToPage("/OrderHistory");
            }

            return Page();
        }
    }
}