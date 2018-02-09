using RolleiShop.Services.Interfaces;
using RolleiShop.Models.Entities;
using System;
using System.Collections.Generic;

namespace RolleiShop.Models.Entities.Order
{
    public class Order : Entity
    {
        private Order() {}

        public Order(string buyerId, Address shipToAddress, List<OrderItem> items)
        {
            ShipToAddress = shipToAddress;
            _orderItems = items;
            BuyerId = buyerId;
        }
        public string BuyerId { get; private set; }

        public DateTimeOffset OrderDate { get; private set; } = DateTimeOffset.Now;
        public Address ShipToAddress { get; private set; }

        private readonly List<OrderItem> _orderItems = new List<OrderItem>();

        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

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
