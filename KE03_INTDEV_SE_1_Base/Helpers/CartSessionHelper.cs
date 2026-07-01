using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace KE03_INTDEV_SE_1_Base.Helpers
{
    public static class CartSessionHelper
    {
        private const string GuestCartKey = "GuestCart";

        public static List<GuestCartItem> GetGuestCart(ISession session)
        {
            string? cartJson = session.GetString(GuestCartKey);

            if (string.IsNullOrWhiteSpace(cartJson))
            {
                return new List<GuestCartItem>();
            }

            return JsonSerializer.Deserialize<List<GuestCartItem>>(cartJson) ?? new List<GuestCartItem>();
        }

        public static void SaveGuestCart(ISession session, List<GuestCartItem> cartItems)
        {
            string cartJson = JsonSerializer.Serialize(cartItems);
            session.SetString(GuestCartKey, cartJson);
        }

        public static void AddToGuestCart(ISession session, int productId, int amount)
        {
            if (amount < 1)
            {
                amount = 1;
            }

            var cartItems = GetGuestCart(session);

            var existingItem = cartItems.FirstOrDefault(item => item.ProductId == productId);

            if (existingItem == null)
            {
                cartItems.Add(new GuestCartItem
                {
                    ProductId = productId,
                    Amount = amount
                });
            }
            else
            {
                existingItem.Amount += amount;
            }

            SaveGuestCart(session, cartItems);
        }

        public static void UpdateGuestCartAmount(ISession session, int productId, int amount)
        {
            if (amount < 1)
            {
                amount = 1;
            }

            var cartItems = GetGuestCart(session);

            var existingItem = cartItems.FirstOrDefault(item => item.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Amount = amount;
            }

            SaveGuestCart(session, cartItems);
        }

        public static void RemoveFromGuestCart(ISession session, int productId)
        {
            var cartItems = GetGuestCart(session);

            cartItems.RemoveAll(item => item.ProductId == productId);

            SaveGuestCart(session, cartItems);
        }

        public static void ClearGuestCart(ISession session)
        {
            session.Remove(GuestCartKey);
        }
    }
}