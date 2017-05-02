using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Web.Http;

namespace PerfectChannelShoppingCart.Controllers
{
    public class ItemController : ApiController
    {
        private readonly IItemRepo _itemRepo;

        public ItemController()
        {
            _itemRepo = new ItemRepo();
        }

        public ItemController(IItemRepo itemRepo)
        {
            _itemRepo = itemRepo;
        }

        public IHttpActionResult Get()
        {
            var items = _itemRepo.Get();
            return Ok(items);
        }
    }

    public class ItemRepo : IItemRepo
    {
        public static List<Item> Items = new List<Item>
            {
                new Item {Id = 1, Name = "Apples", Description = "Fruit", Stock = 5, Price = 2.5m},
                new Item {Id = 2, Name = "Bread", Description = "Loaf", Stock = 10, Price = 1.35m},
                new Item {Id = 3, Name = "Oranges", Description = "Fruit", Stock = 3, Price = 2.99m},
                new Item {Id = 4, Name = "Milk", Description = "Milk", Stock = 10, Price = 2.07m},
                new Item {Id = 5, Name = "Chocolate", Description = "Bars", Stock = 20, Price = 1.79m}
            };

        public  IEnumerable<Item> Get()
        {
            return Items.OrderBy(item=>item.Name);
        }
    }

    public interface IItemRepo
    {
        IEnumerable<Item> Get();
    }

    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }
    }
}
