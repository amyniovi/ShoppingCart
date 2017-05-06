using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using PerfectChannelShoppingCart.Controllers;
using PerfectChannelShoppingCart.Models;
using PerfectChannelShoppingCart.PChannel.Interfaces;

namespace PerfectChannelShoppingCart.PChannel.Repositories
{
    public class CartRepo : ICartRepo
    {
        //Logging in creates a Cart for the username
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

        public Cart Update(IEnumerable<KeyValuePair<int, int>> itemQuantityKeyValuePair)
        {
            throw new NotImplementedException();
        }
    }
}