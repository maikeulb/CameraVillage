using RolleiShop.Entities;
using System.Linq;
using Xunit;

namespace RolleiShop.Tests.Entities.CartItemTests
{
    public class IncrementQuantity
    {
        private decimal _testUnitPrice = 1.23m;
        private int _testCatalogItemId = 123;
        private int _testQuantity = 1;

        [Fact]
        public void IncrementCartItemQuantityOnce()
        {
            var cartItem = CartItem.Create (_testUnitPrice, _testQuantity, _testCatalogItemId);
            cartItem.IncrementQuantity();

            Assert.Equal(2, cartItem.Quantity);
        }

        [Fact]
        public void IncrementCartItemQuantityTwice()
        {
            var cartItem = CartItem.Create(_testUnitPrice, _testQuantity, _testCatalogItemId);
            cartItem.IncrementQuantity();
            cartItem.IncrementQuantity();

            Assert.Equal(3, cartItem.Quantity);
        }

    }
}
