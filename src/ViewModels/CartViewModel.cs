using System;
using System.Linq;
using System.Collections.Generic;

namespace RolleiShop.ViewModels
{
    public class CartViewModel
    {
        public int Id { get; set; }
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public string BuyerId { get; set; }
        public decimal Total()
        {
            return Math.Round(Items.Sum(x => x.UnitPrice * x.Quantity), 2);
        }

        public class CartItem
        {
            public int Id { get; set; }
            public int CatalogItemId { get; set; }
            public string ProductName { get; set; }
            public decimal UnitPrice { get; set; }
            public decimal OldUnitPrice { get; set; }
            public int Quantity { get; set; }
            public string ImageUrl { get; set; }
        }
    }
}
