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
using EnsureThat;

namespace RolleiShop.Services
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUrlComposer _urlComposer;

        public CartService(ApplicationDbContext context,
            IUrlComposer urlComposer)
        {
            _context = context;
            _urlComposer = urlComposer;
        }

        public async Task AddItemToCart(int cartId, int catalogItemId, decimal price, int quantity)
        {
            var cart = await _context.Carts.FindAsync(cartId);

            cart.AddItem(catalogItemId, price, quantity);

            _context.Entry(cart).State = EntityState.Modified;
            await _context.SaveChangesAsync();

        }

        public async Task RemoveItemFromCart(int cartId, int catalogItemId)
        {
            var cart = await _context.Carts.FindAsync(cartId);

            cart.RemoveItem(catalogItemId);

            _context.Entry(cart).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCartAsync(int cartId)
        {
            var cart = await _context.Carts.FindAsync(cartId);

            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetCartItemCountAsync(string userName)
        {
            Ensure.String.IsNotNullOrWhiteSpace(userName, nameof(userName));
            var cartSpec = new CartWithItemsSpecification(userName);
            var cart = (await ListAsync(cartSpec)).FirstOrDefault();
            if (cart == null)
                return 0;
            int count = cart.Items.Sum(i => i.Quantity);
            return count;
        }

        public async Task SetQuantities(int cartId, Dictionary<string, int> quantities)
        {
            EnsureArg.IsNotNull(quantities, nameof(quantities));
            var cart = await _context.Carts.FindAsync(cartId);
            EnsureArg.IsNotNull(cart, nameof(cart));
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

        public async Task TransferCartAsync(string anonymousId, string userName)
        {
            Ensure.String.IsNotNullOrWhiteSpace(anonymousId, nameof(anonymousId));
            Ensure.String.IsNotNullOrWhiteSpace(userName, nameof(userName));
            var cartSpec = new CartWithItemsSpecification(anonymousId);
            var cart = (await ListAsync(cartSpec)).FirstOrDefault();
            if (cart == null) return;
            cart.TransferCart(userName);
            await UpdateAsync(cart);
        }

        private async Task<List<Cart>> ListAsync(ISpecification<Cart> spec)
        {
            var queryableResultWithIncludes = spec.Includes
                .Aggregate(_context.Carts.AsQueryable(),
                    (current, include) => current.Include(include));
            var secondaryResult = spec.IncludeStrings
                .Aggregate(queryableResultWithIncludes,
                    (current, include) => current.Include(include));
            return await secondaryResult
                            .Where(spec.Criteria)
                            .ToListAsync();
        }

        public async Task UpdateAsync(Cart entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
