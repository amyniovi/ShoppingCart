using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PerfectChannelShoppingCart.Controllers
{
    [RoutePrefix("api/Route")]
    public class OrderController : ApiController
    {
        private readonly IItemRepo _itemRepo;

        public OrderController()
        {
            _itemRepo = new ItemRepo();
        }

        public OrderController(IItemRepo itemRepo)
        {
            _itemRepo = itemRepo;
        }

        [Route("cart/{username}/Order")]
        public IHttpActionResult Post([FromBody] Dictionary<string, int> itemQtyDictionary)
        {//This part should be refactored really ...too much responsibility for this Post.
            var invoice = new Invoice();
            List<OrderedItem> orderedItems = new List<OrderedItem>();
            foreach (var itemQty in itemQtyDictionary)
            {
                var item = _itemRepo.GetbyName(itemQty.Key);
                orderedItems.Add(new OrderedItem() { Name = itemQty.Key, Quantity = itemQty.Value, TotalPrice = itemQty.Value * item.Price });
                item.Stock -= itemQty.Value;
                if (item.Stock < 0)
                    return BadRequest("item " + item.Name + ": " + item.Uri + "is currently unavailable") ;
            }

            invoice.OrderedItems = orderedItems;
            invoice.TotalPrice = orderedItems.Sum(item => item.TotalPrice);
            return Ok(invoice);
        }
    }

    public class Invoice
    {
        public decimal TotalPrice { get; set; }
        public List<OrderedItem> OrderedItems { get; set; }
    }
    public class OrderedItem
    {
        public string Name;
        public int Quantity;
        public decimal TotalPrice;
    }
}
