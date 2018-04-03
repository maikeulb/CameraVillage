using RolleiShop.Entities;
using System.Linq;
using Xunit;

namespace RolleiShop.Tests.Entities.CartTests
{
    public class TransferCart
    {
        private string _initialBuyerId = "initial_buyer_id";
        private string _newBuyerId = "newbuyer_id";

        [Fact]
        public void TransferCartToNewID()
        {
            var cart = Cart.Create(_initialBuyerId);
            cart.TransferCart(_newBuyerId);
            Assert.Equal(_newBuyerId, cart.BuyerId);
        }
    }
}
