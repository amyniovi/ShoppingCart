using System;

namespace PerfectChannelShoppingCart.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal PricePerUnit { get; set; }
        public string Info { get; set; } = String.Empty;
        public int Qty { get; set; }
    }
}