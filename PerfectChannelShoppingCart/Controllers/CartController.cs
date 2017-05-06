using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using PerfectChannelShoppingCart.Models;
using PerfectChannelShoppingCart.PChannel.Factories;
using PerfectChannelShoppingCart.PChannel.Interfaces;
using PerfectChannelShoppingCart.PChannel.Repositories;

namespace PerfectChannelShoppingCart.Controllers
{
    [RoutePrefix("api/cart")]
    public class CartController : ApiController
    {
        private readonly ICartRepo _cartRepo;
        private readonly IItemRepo _itemRepo;
        public const string OutOfStockText = "This item is currently unavailable";
        //there should be functionality to check whether user is logged in 
        /* if (Request.Headers?.GetCookies("username") == null)
                 return NotFound();
                 */
        public CartController()
        {
            _cartRepo = new CartRepo();
            _itemRepo = new ItemRepo();
        }

        public CartController(ICartRepo cartRepo, IItemRepo itemRepo)
        {
            _cartRepo = cartRepo;
            _itemRepo = itemRepo;
        }
        /// <summary>
        /// Gets a cart by username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [Route("{username}")]
        public IHttpActionResult Get(string username)
        {
            Cart cart;
            try
            {
                cart = _cartRepo.GetByUserName(username);
            }
            catch
            {
                return InternalServerError();
            }

            return Ok(cart);

        }
        /// <summary>
        /// Creates a cart
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [Route("{username}")]
        public IHttpActionResult Post(string username)
        {
            try
            {
                _cartRepo.AddByUserName(username);
            }
            catch
            {
                return InternalServerError();
            }

            return Ok(_cartRepo.GetByUserName(username));
        }

        /// <summary>
        /// Updates a cart with an item. Many posts should return to many additions hence not using PUT.(not idempotent)
        /// </summary>
        [Route("{username}/item/{id}/{qty}")]
        public IHttpActionResult Post(int id, int qty, string username)
        {
            var item = _itemRepo.GetbyId(id);
            if (item == null) return NotFound();
           
            //checks and adds
            _cartRepo.AddByUserName(username);
            var cart = _cartRepo.GetByUserName(username);
            if (!item.IsEligibleForCart())
            {
                item.Info = $"Only {item.Stock} items in Stock. ";
                return Ok(cart);
            }
            var list = cart.Items.ToList();
            if (list.All(x => x.Id != id))
            {
                list.Add(ItemDtoFactory.Create(item, qty));
            }
            else
            {
                var itemDto = list.FirstOrDefault(i => i.Id == id);
                if(itemDto != null)
                itemDto.Qty = qty;
            }
            cart.Items = list;
            return Ok(cart);
        }

        /// <summary>
        /// Updates a cart with an item. Many posts should return to many additions hence not using PUT.(not idempotent)
        /// </summary>
        [Route("{username}/item/{id}/{qty}")]
        public IHttpActionResult Post(string itemName, int qty, string username)
        {
            var item = _itemRepo.GetbyName(itemName);
            return Post(item.Id, qty, username);
        }
    }

    public static class EligibleItemDelegates
    {
        public static Predicate<Item> InStock = new Predicate<Item>(item => item.Stock > 0);
        public static List<Predicate<Item>> AddToCartRules = new List<Predicate<Item>>();

        static EligibleItemDelegates()
        {
            AddToCartRules.Add(InStock);
        }
    }
}
