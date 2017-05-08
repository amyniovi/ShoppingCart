using PerfectChannelShoppingCart.Models;

namespace PerfectChannelShoppingCart.PChannel.Interfaces
{
    public interface ICartRepo
    {
        Cart GetByUserName(string username);
        void AddByUserName(string username);
        Cart UpdateSingleItem(string username, CartItem cartItem);

    }
}