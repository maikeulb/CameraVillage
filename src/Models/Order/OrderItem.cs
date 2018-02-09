using Rolleishop.Services.Interfaces;
using Rolleishop.Models.Entities;
using System;
using System.Collections.Generic;

namespace RolleiShop.Models.Entities.Order
{
    public class OrderItem : Entity
    {
        public CatalogItemOrdered ItemOrdered { get; private set; }
        public decimal UnitPrice { get; private set; }
        public int Units { get; private set; }

        protected OrderItem() {}
        public OrderItem(CatalogItemOrdered itemOrdered, decimal unitPrice, int units)
        {
            ItemOrdered = itemOrdered;
            UnitPrice = unitPrice;
            Units = units;
        }
    }
}
