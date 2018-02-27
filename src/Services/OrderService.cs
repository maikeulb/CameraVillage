using RolleiShop.Services.Interfaces;
using RolleiShop.Models.Entities;
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
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateOrderAsync(int cartId)
        {
            var cart = await _context.Set<Cart>().FindAsync(cartId);

            var items = new List<OrderItem>();
            foreach (var item in cart.Items)
            {
                var catalogItem = await _context.Set<CatalogItem>().FindAsync(item.CatalogItemId);
                var itemOrdered = new CatalogItemOrdered(catalogItem.Id, catalogItem.Name, catalogItem.ImageUrl);
                var orderItem = new OrderItem(itemOrdered, item.UnitPrice, item.Quantity);
                items.Add(orderItem);
            }
            var order = Order.Create(cart.BuyerId, items);

            _context.Set<Order>().Add(order);
            await _context.SaveChangesAsync();
        }
    }
}
