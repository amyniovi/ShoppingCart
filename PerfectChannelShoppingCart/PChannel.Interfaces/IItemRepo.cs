using System.Collections.Generic;
using PerfectChannelShoppingCart.Models;

namespace PerfectChannelShoppingCart.PChannel.Interfaces
{
    public interface IItemRepo
    {
        IEnumerable<Item> Get();
        Item GetbyId(int id);
        Item GetbyName(string itemName);
    }
}