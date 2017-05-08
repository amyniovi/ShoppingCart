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
      /// This method updates the cart with a total count of "qty" for that item id.
      /// </summary>
      /// <param name="id"></param>
      /// <param name="qty"></param> total quantity for the items of this id;
      /// <param name="username"></param>
      /// <returns></returns>
        [Route("{username}/item/{id}/{qty}")]
        public IHttpActionResult Post(int id, int qty, string username)
        {
            var item = _itemRepo.GetbyId(id);
            //Adds a cart if one doesnt exist for the user.
            _cartRepo.AddByUserName(username);

            var cart = _cartRepo.GetByUserName(username);
            if (item == null)
                return Ok(cart);
            if (!item.IsEligibleForCart())
            {
                return Ok(cart);
            }
            var cartItem = CartItemFactory.Create(item, qty);
            var updatedCart = _cartRepo.UpdateSingleItem(username, cartItem);

            return Ok(updatedCart);
        }

        /// <summary>
        ///Same as above but finds an item in the cart using its name.
        /// </summary>
        [Route("{username}/item/{id:int}/{qty:int}")]
        public IHttpActionResult Post(string itemName, int qty, string username)
        {
            var item = _itemRepo.GetbyName(itemName);
            return Post(item.Id, qty, username);
        }
    }
}
