using PerfectChannelShoppingCart.Models;

namespace PerfectChannelShoppingCart.PChannel.Factories
{
    /// <summary>
    /// This class maps the properties of the item to convert it to a cartItem.
    /// </summary>
    public static class CartItemFactory
    {
        public static CartItem Create(Item item, int qty)
        {
            if (!item.IsEligibleForCart())
                return null;

            var cartItem = new CartItem()
            {
                Id = item.Id,
                Name = item.Name,
                PricePerUnit = item.Price
            };
            MapQuantityBasedOnStock(cartItem,qty,item.Stock);

            return cartItem;
        }

        public static void MapQuantityBasedOnStock(CartItem cartItem,int qty, int stock)
        {
            cartItem.Qty = qty > stock ? stock: qty;
            cartItem.Info = qty > stock ? $"Only {stock} items in stock. " : string.Empty;
        }
    }
}