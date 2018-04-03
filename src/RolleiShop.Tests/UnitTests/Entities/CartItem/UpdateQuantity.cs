using RolleiShop.Entities;
using System.Linq;
using Xunit;

namespace RolleiShop.Tests.Entities.CartItemTests
{
    public class UpdateQuantity
    {
        private decimal _testUnitPrice = 1.23m;
        private int _testQuantity = 1;
        private int _testCatalogItemId = 123;
        private int _testNewQuantity = 2;

        [Fact]
        public void UpdateCartItemQuantity()
        {
            var cartItem = CartItem.Create (_testUnitPrice, _testQuantity, _testCatalogItemId);
            cartItem.UpdateQuantity(_testNewQuantity);

            Assert.Equal(_testNewQuantity, cartItem.Quantity);
        }
    }
}
