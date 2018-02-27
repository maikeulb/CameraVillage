using RolleiShop.Services.Interfaces;
using RolleiShop.Models.Entities;
using System;
using System.Collections.Generic;

namespace RolleiShop.Models.Entities
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

        public static OrderItem Create (CatalogItemOrdered itemOrdered, decimal unitPrice, int units)
        {
            return new OrderItem(itemOrdered, unitPrice, units);
        }
    }
}
