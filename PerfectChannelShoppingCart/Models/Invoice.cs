using System.Collections.Generic;

namespace PerfectChannelShoppingCart.Models
{
    public class Invoice
    {
        public decimal TotalPrice { get; set; }
        public List<CartItem> OrderedItems { get; set; }
    }
}