using RolleiShop.Services.Interfaces;
using RolleiShop.Models.Entities;
using RolleiShop.Models.Entities.Order;
using RolleiShop.Models.Interfaces;
using RolleiShop.Infra.App;
using RolleiShop.Infra.App.Interfaces;
using RolleiShop.Data.Context;
using RolleiShop.Specifications;
using RolleiShop.Features.Catalog;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;

namespace RolleiShop.Services
{
    public class OrderService : IOrderService
    {
        private readonly IAsyncRepository<Order> _orderRepository;
        private readonly IAsyncRepository<Cart> _cartRepository;
        private readonly IAsyncRepository<CatalogItem> _itemRepository;

        public OrderService(IAsyncRepository<Cart> cartRepository,
            IAsyncRepository<CatalogItem> itemRepository,
            IAsyncRepository<Order> orderRepository)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _itemRepository = itemRepository;
        }

        public async Task CreateOrderAsync(int cartId, Address shippingAddress)
        {
            var cart = await _cartRepository.GetByIdAsync(cartId);
            var items = new List<OrderItem>();
            foreach (var item in cart.Items)
            {
                var catalogItem = await _itemRepository.GetByIdAsync(item.CatalogItemId);
                var itemOrdered = new CatalogItemOrdered(catalogItem.Id, catalogItem.Name, catalogItem.ImageUrl);
                var orderItem = new OrderItem(itemOrdered, item.UnitPrice, item.Quantity);
                items.Add(orderItem);
            }
            var order = new Order(cart.BuyerId, shippingAddress, items);

            await _orderRepository.AddAsync(order);
        }
    }
}
