using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace KE03_INTDEV_SE_1_Base.Pages
{
    public class CheckoutModel : PageModel
    {
        private readonly ICartRepository _cartRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;

        public CheckoutModel(
            ICartRepository cartRepository,
            IOrderRepository orderRepository,
            ICustomerRepository customerRepository)
        {
            _cartRepository = cartRepository;
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
        }

        public IEnumerable<CartItem> CartItems { get; set; } = new List<CartItem>();

        public Customer? SelectedCustomer { get; set; }

        public decimal TotalPrice { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Voornaam is verplicht.")]
        public string FirstName { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Achternaam is verplicht.")]
        public string LastName { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Adres is verplicht.")]
        public string Address { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Huisnummer is verplicht.")]
        public string HouseNumber { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Postcode is verplicht.")]
        public string PostalCode { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Stad is verplicht.")]
        public string City { get; set; } = string.Empty;

        public IActionResult OnGet()
        {
            int? customerId = HttpContext.Session.GetInt32("CustomerId");

            if (!customerId.HasValue)
            {
                return RedirectToPage("/Account");
            }

            LoadCheckoutData(customerId.Value);

            if (!CartItems.Any())
            {
                return RedirectToPage("/Cart");
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            int? customerId = HttpContext.Session.GetInt32("CustomerId");

            if (!customerId.HasValue)
            {
                return RedirectToPage("/Account");
            }

            LoadCheckoutData(customerId.Value);

            if (!CartItems.Any())
            {
                return RedirectToPage("/Cart");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var order = new Order
            {
                CustomerId = customerId.Value,
                OrderDate = DateTime.Now,
                FirstName = FirstName,
                LastName = LastName,
                Address = Address,
                HouseNumber = HouseNumber,
                PostalCode = PostalCode,
                City = City,
                TotalPrice = TotalPrice
            };

            foreach (var cartItem in CartItems)
            {
                order.OrderItems.Add(new OrderItem
                {
                    ProductId = cartItem.ProductId,
                    Amount = cartItem.Amount,
                    PriceAtOrder = cartItem.Product.Price
                });
            }

            _orderRepository.AddOrder(order);

            _cartRepository.ClearCart(customerId.Value);

            return RedirectToPage("/OrderSuccess", new { orderId = order.Id });
        }

        private void LoadCheckoutData(int customerId)
        {
            SelectedCustomer = _customerRepository.GetCustomerById(customerId);

            CartItems = _cartRepository.GetCartItemsByCustomerId(customerId);

            TotalPrice = CartItems.Sum(cartItem => cartItem.Product.Price * cartItem.Amount);
        }
    }
}
