using System.Collections.Generic;
using PerfectChannelShoppingCart.Controllers;
using PerfectChannelShoppingCart.Models;

namespace PerfectChannelShoppingCart.PChannel.Repositories
{
    public interface IItemRepo
    {
        IEnumerable<Item> Get();
        Item GetbyId(int id);
        Item GetbyName(string itemName);
    }
}