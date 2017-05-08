
using PerfectChannelShoppingCart.Models;
using PerfectChannelShoppingCart.PChannel.Interfaces;
using PerfectChannelShoppingCart.PChannel.Repositories;

namespace PerfectChannelShoppingCart.PChannel.Services
{
    public class StockService : IStockService
    {
        private readonly IItemRepo _itemRepo;

        public StockService()
        {
            _itemRepo = new ItemRepo();
        }

        public StockService(IItemRepo itemRepo)
        {
            _itemRepo = itemRepo;
        }
        /// <summary>
        /// In a real application we d need to hit the ItemRepo to update, but as its a collection i skipped that part.
        /// </summary>
        /// <param name="cartItem"></param>
        public void UpdateStock(CartItem cartItem)
        {
            var item = _itemRepo.GetbyId(cartItem.Id);
            if (item.Stock >= cartItem.Qty)
            {
                item.Stock -= cartItem.Qty;
                cartItem.Info = string.Empty;
            }
            else
            {
                cartItem.Qty = item.Stock;
                cartItem.Info = $"There are only {item.Stock} items in Stock currently.";
                item.Stock = 0;
            }
        }
    }
}