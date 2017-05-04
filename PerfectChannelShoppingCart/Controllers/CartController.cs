﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

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
        /// <param name="id"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        [Route("{username}/item/{id}")]
        public IHttpActionResult Post(int id, string username)
        {
            var item = _itemRepo.GetbyId(id);
            if (item == null) return NotFound();
            if (!item.IsEligibleForCart())
                return BadRequest(OutOfStockText);
            //checks and adds
            _cartRepo.AddByUserName(username);
            var cart = _cartRepo.GetByUserName(username);

            var list = cart.Items.ToList();
            list.Add(item);
            cart.Items = list;
            return Ok(cart);
        }

        /// <summary>
        /// Updates a cart with an item. Many posts should return to many additions hence not using PUT.(not idempotent)
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        [Route("{username}/item/{id}")]
        public IHttpActionResult Post(string itemName, string username)
        {
            var item = _itemRepo.GetbyName(itemName);
            return Post(item.Id, username);
        }
    }

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

    public static class EligibleItemDelegates
    {
        public static Predicate<Item> InStock = new Predicate<Item>(item => item.Stock > 0);
        public static List<Predicate<Item>> AddToCartRules = new List<Predicate<Item>>();

        static EligibleItemDelegates()
        {
            AddToCartRules.Add(InStock);
        }
    }

    public interface ICartRepo
    {
        Cart GetByUserName(string username);
        void AddByUserName(string username);

    }

    public class Cart
    {
        public string UniqueId { get; set; }
        public IEnumerable<Item> Items { get; set; } = new List<Item>();
    }

}
