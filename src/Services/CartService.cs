using Microsoft.EntityFrameworkCore;
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
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;

        public CartService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddItemToCart(int cartId, int catalogItemId, decimal price, int quantity)
        {
            var cart = await _context.Set<Cart>().FindAsync(cartId);

            cart.AddItem(catalogItemId, price, quantity);

            _context.Entry(cart).State = EntityState.Modified;
            await _context.SaveChangesAsync();

        }

        public async Task RemoveItemFromCart(int cartId, int catalogItemId)
        {
            var cart = await _context.Set<Cart>().FindAsync(cartId);

            cart.RemoveItem(catalogItemId);

            _context.Entry(cart).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCartAsync(int cartId)
        {
            var cart = await _context.Set<Cart>().FindAsync(cartId);

            _context.Set<Cart>().Remove(cart);
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetCartItemCountAsync(string userName)
        {
            var cartSpec = new CartWithItemsSpecification(userName);
            var cart = (await ListAsync(cartSpec)).FirstOrDefault();
            if (cart == null)
                return 0;
            int count = cart.Items.Sum(i => i.Quantity);
            return count;
        }

        public async Task SetQuantities(int cartId, Dictionary<string, int> quantities)
        {
            var cart = await _context.Set<Cart>().FindAsync(cartId);
            foreach (var item in cart.Items)
            {
                if (quantities.TryGetValue(item.Id.ToString(), out var quantity))
                {
                    item.UpdateQuantity(quantity);
                }
            }

            _context.Entry(cart).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        private async Task<List<Cart>> ListAsync(ISpecification<Cart> spec)
        {
            var queryableResultWithIncludes = spec.Includes
                .Aggregate(_context.Set<Cart>().AsQueryable(),
                    (current, include) => current.Include(include));
            var secondaryResult = spec.IncludeStrings
                .Aggregate(queryableResultWithIncludes,
                    (current, include) => current.Include(include));
            return await secondaryResult
                            .Where(spec.Criteria)
                            .ToListAsync();
        }

    }
}
