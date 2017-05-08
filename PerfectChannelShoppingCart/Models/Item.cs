using System;
using System.Collections.Generic;
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

        public bool IsEligibleForCart()
        {
            return EligibleItemDelegates.AddToCartRules.All(rule => rule(this));
        }
    }
     public static class EligibleItemDelegates
    {
        public static Predicate<Item> InStock = item => item.Stock > 0;
  
        public static List<Predicate<Item>> AddToCartRules = new List<Predicate<Item>>();

        static EligibleItemDelegates()
        {
            AddToCartRules.Add(InStock);
        }
    }
}