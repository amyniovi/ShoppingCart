using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Web.Http;

namespace PerfectChannelShoppingCart.Controllers
{
    public class CartController : ApiController
    {
        private readonly ICartRepo _cartRepo;

        public CartController()
        {
            _cartRepo = new CartRepo();
        }

        public CartController(ICartRepo cartRepo)
        {
            _cartRepo = cartRepo;
        }

        public IHttpActionResult Get()
        {
            return Ok();
        }
    }

    public class CartRepo : ICartRepo
    {
        public static ConcurrentDictionary<string, Cart> Carts = new ConcurrentDictionary<string, Cart>();
        public Cart GetByUserName(string username)
        {
            Cart cart = null;
            Carts.TryGetValue(username, out cart);
            return cart;
        }

        public void AddByUserName(string username)
        {
           var cart = new Cart() {UniqueId = username};
            Carts.TryAdd(username, cart);
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
        public IEnumerable<Item> Items { get; set; }
    }
}
