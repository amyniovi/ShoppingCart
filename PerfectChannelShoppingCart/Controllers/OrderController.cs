using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PerfectChannelShoppingCart.Models;
using PerfectChannelShoppingCart.PChannel.Interfaces;
using PerfectChannelShoppingCart.PChannel.Repositories;

namespace PerfectChannelShoppingCart.Controllers
{
    [RoutePrefix("api/order")]
    public class OrderController : ApiController
    {
        private readonly IItemRepo _itemRepo;
        private readonly ICartRepo _cartRepo;

        public OrderController()
        {
            _itemRepo = new ItemRepo();
            _cartRepo = new CartRepo();
        }

        public OrderController(IItemRepo itemRepo, ICartRepo cartRepo)
        {
            _itemRepo = itemRepo;
            _cartRepo = cartRepo;
        }

        [Route("cart/{username}")]
        public IHttpActionResult Get(string username )
        {//This part should be refactored really ...too much responsibility for this Get.
            var invoice = new Invoice();
            List<CartItemDto> orderedItems = new List<CartItemDto>();
            var cart = _cartRepo.GetByUserName(username);
            foreach (var cartItem in cart.Items)
            {
                var item = _itemRepo.GetbyId(cartItem.Id);
                if (item.Stock < cartItem.Qty)
                {
                    item.Info = $"Only {item.Stock} items are available";
                }
                else
                    item.Stock -= cartItem.Qty;
               
                orderedItems.Add(cartItem);
               
            }

            invoice.OrderedItems = orderedItems;
            invoice.TotalPrice = orderedItems.Sum(item => item.PricePerUnit * item.Qty);
            return Ok(invoice);
        }
    }

    public class Invoice
    {
        public decimal TotalPrice { get; set; }
        public List<CartItemDto> OrderedItems { get; set; }
    }
   
}
