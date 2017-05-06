using PerfectChannelShoppingCart.Controllers;
using PerfectChannelShoppingCart.Models;

namespace PerfectChannelShoppingCart.PChannel.Factories
{
    public static class ItemDtoFactory
    {
        public static CartItemDto Create(Item item, int qty)
        {
            return  new CartItemDto()
            {
                Id =  item.Id, 
                Name = item.Name,
                Qty = qty,
                PricePerUnit = item.Price
            };
        }
    }
}