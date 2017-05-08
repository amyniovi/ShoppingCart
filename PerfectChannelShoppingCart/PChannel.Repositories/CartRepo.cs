using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using PerfectChannelShoppingCart.Controllers;
using PerfectChannelShoppingCart.Models;
using PerfectChannelShoppingCart.PChannel.Factories;
using PerfectChannelShoppingCart.PChannel.Interfaces;

namespace PerfectChannelShoppingCart.PChannel.Repositories
{
    public class CartRepo : ICartRepo
    {
        public static ConcurrentDictionary<string, Cart> Carts = new ConcurrentDictionary<string, Cart>(StringComparer.OrdinalIgnoreCase);
        public Cart GetByUserName(string username)
        {
            Cart cart = null;
            Carts.TryGetValue(username, out cart);
            return cart;
        }

        public void AddByUserName(string username)
        {
            var cart = new Cart() { UniqueId = username };
            Carts.TryAdd(username, cart);
        }

        public Cart UpdateSingleItem(string username, CartItem cartItem)
        {
            var cart = GetByUserName(username);
            var list = cart.Items.ToList();
            if (list.Any(x=> x.Id==cartItem.Id))
                list.Remove(list.Find(x=>x.Id==cartItem.Id));
            list.Add(cartItem);
            Carts.TryUpdate(username,new Cart() {UniqueId = username,Items = list},cart );
            return GetByUserName(username);
        }
    }
}