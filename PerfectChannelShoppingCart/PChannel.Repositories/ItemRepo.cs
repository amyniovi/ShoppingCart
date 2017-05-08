using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using PerfectChannelShoppingCart.Models;
using PerfectChannelShoppingCart.PChannel.Interfaces;

namespace PerfectChannelShoppingCart.PChannel.Repositories
{
    public class ItemRepo : IItemRepo
    {
        //This should be a concurrent Dictionary just tried to not over-engineer.
        //This would not be thread safe as is.
        //We have the Carts dictionary that is static and concurrent,in CartRepo.
        public static ConcurrentBag<Item> Items = new ConcurrentBag<Item>()
        {
            new Item {Id = 1, Name = "Apples", Description = "Fruit", Stock = 5, Price = 2.5m},
            new Item {Id = 2, Name = "Bread", Description = "Loaf", Stock = 10, Price = 1.35m},
            new Item {Id = 3, Name = "Oranges", Description = "Fruit", Stock = 0, Price = 2.99m},
            new Item {Id = 4, Name = "Milk", Description = "Milk", Stock = 10, Price = 2.07m},
            new Item {Id = 5, Name = "Chocolate", Description = "Bars", Stock = 20, Price = 1.79m}
        };

        public  IEnumerable<Item> Get()
        {
            return Items.OrderBy(item=>item.Name);
        }

        public Item GetbyId(int id)
        {
            return Items.FirstOrDefault(item => item.Id == id);
        }

        public Item GetbyName(string itemName)
        {
            return Items.FirstOrDefault(item => String.Equals(item.Name, itemName, StringComparison.InvariantCultureIgnoreCase));
        }

    }
}