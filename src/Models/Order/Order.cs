using RolleiShop.Services.Interfaces;
using RolleiShop.Models.Entities;
using System;
using System.Collections.Generic;

namespace RolleiShop.Models.Entities
{
    public class Order : Entity
    {
        public string BuyerId { get; private set; }
        public DateTimeOffset OrderDate { get; private set; } = DateTimeOffset.Now;
        public Address ShipToAddress { get; private set; }
        private readonly List<OrderItem> _orderItems = new List<OrderItem>();
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

        private Order() {}

        private Order(string buyerId, Address shipToAddress, List<OrderItem> items)
        {
            ShipToAddress = shipToAddress;
            _orderItems = items;
            BuyerId = buyerId;
        }

        public static Order Create (string buyerId, Address shipToAddress, List<OrderItem> items)
        {
            return new Order(buyerId, shipToAddress, items);
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
