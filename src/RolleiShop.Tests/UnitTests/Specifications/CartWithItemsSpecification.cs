using RolleiShop.Specifications;
using RolleiShop.Entities;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace RolleiShop.Tests.Specifications.CartWithItemsSpecifications
{
    public class CartWithItems
    {
        private string _testBuyerId = "first";
        private string _testSecondBuyerId = "second";
        private string _testThirdBuyerId = "third";

        [Fact]
        public void MatchesCartWithGivenBuyerId()
        {
            var spec = new CartWithItemsSpecification(_testBuyerId);

            var result = GetTestCartCollection()
                .AsQueryable()
                .FirstOrDefault(spec.Criteria);

            Assert.NotNull(result);
            Assert.Equal(_testBuyerId, result.BuyerId);
        }

        [Fact]
        public void MatchesNoCartIfIdNotPresent()
        {
            int badId = -1;
            var spec = new CartWithItemsSpecification(badId);

            Assert.False(GetTestCartCollection()
                .AsQueryable()
                .Any(spec.Criteria));
        }

        public List<Cart> GetTestCartCollection()
        {
            return new List<Cart>()
            {
                Cart.Create(_testBuyerId),
                Cart.Create(_testSecondBuyerId),
                Cart.Create(_testThirdBuyerId)
            };
        }
    }
}
