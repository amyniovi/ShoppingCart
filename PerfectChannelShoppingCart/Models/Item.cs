using System.Linq;
using PerfectChannelShoppingCart.Controllers;

namespace PerfectChannelShoppingCart.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }
        public string Uri { get; set; }
        public string Info { get; set; }

        public bool IsEligibleForCart()
        {
            return EligibleItemDelegates.AddToCartRules.All(rule => rule(this));
        }
    }
}