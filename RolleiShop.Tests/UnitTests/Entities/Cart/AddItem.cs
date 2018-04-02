using RolleiShop.Entities;
using System.Linq;
using Xunit;

namespace RolleiShop.Tests.Entities.CartTests
{
    public class AddItem
    {
        private string _testBuyerId = "test_buyer_id";
        private int _testCatalogItemId = 123;
        private decimal _testUnitPrice = 1.23m;
        private int _testQuantity = 1;

        [Fact]
        public void AddsCartItemIfNotPresent()
        {
            var cart = Cart.Create(_testBuyerId);
            cart.AddItem(_testCatalogItemId, _testUnitPrice, _testQuantity);

            var firstItem = cart.Items.Single();
            Assert.Equal(_testCatalogItemId, firstItem.CatalogItemId);
            Assert.Equal(_testUnitPrice, firstItem.UnitPrice);
            Assert.Equal(_testQuantity, firstItem.Quantity);
        }

        [Fact]
        public void IncrementsQuantityOfItemIfPresent()
        {
            var cart = Cart.Create(_testBuyerId);
            cart.AddItem(_testCatalogItemId, _testUnitPrice, _testQuantity);
            cart.AddItem(_testCatalogItemId, _testUnitPrice, _testQuantity);

            var firstItem = cart.Items.Single();
            Assert.Equal(_testQuantity*2, firstItem.Quantity);
        }

        [Fact]
        public void KeepsOriginalUnitPriceIfMoreItemsAdded()
        {
            var cart = Cart.Create(_testBuyerId);
            cart.AddItem(_testCatalogItemId, _testUnitPrice, _testQuantity);
            cart.AddItem(_testCatalogItemId, _testUnitPrice * 2, _testQuantity);

            var firstItem = cart.Items.Single();
            Assert.Equal(_testUnitPrice, firstItem.UnitPrice);
        }

        [Fact]
        public void DefaultsToQuantityOfOne()
        {
            var cart = Cart.Create(_testBuyerId);
            cart.AddItem(_testCatalogItemId, _testUnitPrice);

            var firstItem = cart.Items.Single();
            Assert.Equal(1, firstItem.Quantity);
        }
    }
}
