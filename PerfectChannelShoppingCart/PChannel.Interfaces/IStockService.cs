using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PerfectChannelShoppingCart.Models;

namespace PerfectChannelShoppingCart.PChannel.Interfaces
{
    public interface IStockService
    {
         void UpdateStock(CartItem cartItem);
    }
}