using RolleiShop.Services.Interfaces;
using RolleiShop.Entities;
using System;
using System.Collections.Generic;

namespace RolleiShop.Entities
{
    public class Order : Entity
    {
        public string BuyerId { get; private set; }
        public DateTimeOffset OrderDate { get; private set; } = DateTimeOffset.Now;
        private readonly List<OrderItem> _orderItems = new List<OrderItem>();
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

        private Order() {}

        private Order(string buyerId, List<OrderItem> items)
        {
            _orderItems = items;
            BuyerId = buyerId;
        }

        public static Order Create (string buyerId, List<OrderItem> items)
        {
            return new Order(buyerId, items);
        }

        public decimal Total()
        {
            var total = 0m;
            foreach (var item in _orderItems)
            {
                total += item.UnitPrice * item.Units;
            }
            return total;
        }
    }
}
