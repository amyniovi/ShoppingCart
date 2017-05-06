using System.Collections.Generic;
using PerfectChannelShoppingCart.Controllers;

namespace PerfectChannelShoppingCart.Models
{
    public class Cart
    {
        public string UniqueId { get; set; }
        public IEnumerable<CartItemDto> Items { get; set; } = new List<CartItemDto>();
    }
}