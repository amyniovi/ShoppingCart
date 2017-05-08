using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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