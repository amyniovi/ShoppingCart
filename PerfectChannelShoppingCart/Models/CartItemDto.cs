using System;

namespace PerfectChannelShoppingCart.Models
{
    public class CartItemDto
    {
        public int Id { get; set; }
        public decimal PricePerUnit { get; set; }
        public string Info { get; set; } = String.Empty;
        public int Qty { get; set; }
        public string Name { get; set; }
    }
}