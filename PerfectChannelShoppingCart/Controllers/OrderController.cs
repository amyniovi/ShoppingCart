using System;
using System.Linq;
using System.Web.Http;
using PerfectChannelShoppingCart.Models;
using PerfectChannelShoppingCart.PChannel.Interfaces;
using PerfectChannelShoppingCart.PChannel.Repositories;
using PerfectChannelShoppingCart.PChannel.Services;

namespace PerfectChannelShoppingCart.Controllers
{
    [RoutePrefix("api/order")]
    public class OrderController : ApiController
    {
        private readonly ICartRepo _cartRepo;
        private readonly IStockService _stockService;

        public OrderController()
        {
            _cartRepo = new CartRepo();
            _stockService = new StockService();
        }

        public OrderController(ICartRepo cartRepo, IStockService stockService)
        {
            _cartRepo = cartRepo;
            _stockService = stockService;
        }

        [Route("cart/{username}")]
        public IHttpActionResult Get(string username)
        {
            var invoice = new Invoice();
            var cart = _cartRepo.GetByUserName(username);
            if (!cart.IsCheckedOut)
            {
                foreach (var cartItem in cart.Items)
                {
                    _stockService.UpdateStock(cartItem);
                }
            }

            cart.IsCheckedOut = true;
            invoice.OrderedItems = cart.Items.ToList();
            invoice.TotalPrice = cart.Items.Sum(item => item.PricePerUnit * item.Qty);
            return Ok(invoice);
        }
    }
}
