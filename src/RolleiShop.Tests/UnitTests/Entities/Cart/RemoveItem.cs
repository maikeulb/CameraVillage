/* using RolleiShop.Entities; */
/* using System.Linq; */
/* using Xunit; */

/* namespace RolleiShop.Tests.Entities.CartTests */
/* { */
/*     public class RemoveItem */
/*     { */
/*         private string _testBuyerId = "test_buyer_id"; */
/*         private int _testCatalogItemId = 123; */
/*         private decimal _testUnitPrice = 1.23m; */
/*         private int _testQuantity = 1; */

        /* [Fact] */
        /* public void AddTwoCartItemsAndRemovesOne() */
        /* { */
        /*     var cart = Cart.Create(_testBuyerId); */
        /*     cart.AddItem(_testCatalogItemId, _testUnitPrice, _testQuantity); */
        /*     cart.AddItem(_testCatalogItemId, _testUnitPrice, _testQuantity); */
        /*     cart.RemoveItem(_testCatalogItemId); */

/*             var firstItem = cart.Items.Single(); */
/*             Assert.Equal(_testQuantity, firstItem.Quantity); */
/*         } */

        /* [Fact] */
        /* public void AddOneCartItemsAndRemovesOne() */
        /* { */
        /*     var cart = Cart.Create(_testBuyerId); */
        /*     cart.AddItem(_testCatalogItemId, _testUnitPrice, _testQuantity); */
        /*     cart.RemoveItem(_testCatalogItemId); */

/*             var firstItem = cart.Items.Single(); */
/*             Assert.Equal(0, firstItem.Quantity); */
/*         } */
/*     } */
/* } */
