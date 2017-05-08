using System.Collections.Generic;

namespace PerfectChannelShoppingCart.Models
{
    public class Cart
    {
        public string UniqueId { get; set; }
        public IEnumerable<CartItem> Items { get; set; } = new List<CartItem>();
        public bool IsCheckedOut { get; set; }
        public string Uri { get; set; }
    }
}