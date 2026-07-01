using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using KE03_INTDEV_SE_1_Base.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KE03_INTDEV_SE_1_Base.Pages
{
    public class CartModel : PageModel
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;

        public CartModel(
            ICartRepository cartRepository,
            ICustomerRepository customerRepository,
            IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _customerRepository = customerRepository;
            _productRepository = productRepository;
        }

        public List<CartViewItem> CartItems { get; set; } = new();

        public Customer? SelectedCustomer { get; set; }

        public decimal TotalPrice { get; set; }

        public void OnGet()
        {
            LoadCart();
        }

        public IActionResult OnPostUpdateAmount(int productId, int amount)
        {
            int? customerId = HttpContext.Session.GetInt32("CustomerId");

            if (customerId.HasValue)
            {
                _cartRepository.UpdateAmount(customerId.Value, productId, amount);
            }
            else
            {
                CartSessionHelper.UpdateGuestCartAmount(HttpContext.Session, productId, amount);
            }

            return RedirectToPage("/Cart");
        }

        public IActionResult OnPostRemoveFromCart(int productId)
        {
            int? customerId = HttpContext.Session.GetInt32("CustomerId");

            if (customerId.HasValue)
            {
                _cartRepository.RemoveFromCart(customerId.Value, productId);
            }
            else
            {
                CartSessionHelper.RemoveFromGuestCart(HttpContext.Session, productId);
            }

            return RedirectToPage("/Cart");
        }

        private void LoadCart()
        {
            int? customerId = HttpContext.Session.GetInt32("CustomerId");

            if (customerId.HasValue)
            {
                SelectedCustomer = _customerRepository.GetCustomerById(customerId.Value);

                CartItems = _cartRepository.GetCartItemsByCustomerId(customerId.Value)
                    .Select(cartItem => new CartViewItem
                    {
                        ProductId = cartItem.ProductId,
                        Product = cartItem.Product,
                        Amount = cartItem.Amount
                    })
                    .ToList();
            }
            else
            {
                var guestCartItems = CartSessionHelper.GetGuestCart(HttpContext.Session);

                foreach (var guestItem in guestCartItems)
                {
                    var product = _productRepository.GetProductById(guestItem.ProductId);

                    if (product != null)
                    {
                        CartItems.Add(new CartViewItem
                        {
                            ProductId = guestItem.ProductId,
                            Product = product,
                            Amount = guestItem.Amount
                        });
                    }
                }
            }

            TotalPrice = CartItems.Sum(cartItem => cartItem.Product.Price * cartItem.Amount);
        }
    }

    public class CartViewItem
    {
        public int ProductId { get; set; }

        public Product Product { get; set; } = null!;

        public int Amount { get; set; }
    }
}