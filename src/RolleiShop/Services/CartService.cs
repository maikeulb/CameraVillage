using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EnsureThat;
using Microsoft.EntityFrameworkCore;
using RolleiShop.Data.Context;
using RolleiShop.Entities;
using RolleiShop.Infrastructure.Interfaces;
using RolleiShop.Services.Interfaces;
using RolleiShop.Specifications;
using RolleiShop.Specifications.Interfaces;

namespace RolleiShop.Services
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;

        public CartService (ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddItemToCart (int cartId, int catalogItemId, decimal price, int quantity)
        {
            var cart = await _context.Carts.FindAsync (cartId);

            cart.AddItem (catalogItemId, price, quantity);

            _context.Entry (cart).State = EntityState.Modified;
            await _context.SaveChangesAsync ();
        }

        public async Task RemoveItemFromCart (int cartId, int catalogItemId)
        {
            var cart = await _context.Carts.FindAsync (cartId);

            cart.RemoveItem (catalogItemId);

            _context.Entry (cart).State = EntityState.Modified;
            await _context.SaveChangesAsync ();
        }

        public async Task DeleteCartAsync (int cartId)
        {
            var cart = await _context.Carts.FindAsync (cartId);

            _context.Carts.Remove (cart);
            await _context.SaveChangesAsync ();
        }

        public async Task<int> GetCartItemCountAsync (string userName)
        {
            Ensure.String.IsNotNullOrWhiteSpace (userName, nameof (userName));
            var cartSpec = new CartWithItemsSpecification (userName);
            var cart = (await ListAsync (cartSpec)).FirstOrDefault ();
            if (cart == null)
                return 0;
            int count = cart.Items.Sum (i => i.Quantity);
            return count;
        }

        public async Task SetQuantities (int cartId, Dictionary<string, int> quantities)
        {
            EnsureArg.IsNotNull (quantities, nameof (quantities));
            var cart = await _context.Carts.FindAsync (cartId);
            EnsureArg.IsNotNull (cart, nameof (cart));
            foreach (var item in cart.Items)
            {
                if (quantities.TryGetValue (item.Id.ToString (), out var quantity))
                {
                    item.UpdateQuantity (quantity);
                }
            }
            _context.Entry (cart).State = EntityState.Modified;
            await _context.SaveChangesAsync ();
        }

        public async Task TransferCartAsync (string anonymousId, string userName)
        {
            Ensure.String.IsNotNullOrWhiteSpace (anonymousId, nameof (anonymousId));
            Ensure.String.IsNotNullOrWhiteSpace (userName, nameof (userName));
            var cartSpec = new CartWithItemsSpecification (anonymousId);
            var cart = (await ListAsync (cartSpec)).FirstOrDefault ();
            if (cart == null) return;
            cart.TransferCart (userName);
            await UpdateAsync (cart);
        }

        public async Task UpdateAsync (Cart entity)
        {
            _context.Entry (entity).State = EntityState.Modified;
            await _context.SaveChangesAsync ();
        }

        public async Task<IEnumerable<string>> GetUsers ()
        {
            return await _context.Carts.Select (c => c.BuyerId).ToListAsync ();
        }

        public async Task<Cart> GetCartAsync (string userName)
        {
            Ensure.String.IsNotNullOrWhiteSpace (userName, nameof (userName));
            var cartSpec = new CartWithItemsSpecification (userName);
            return (await ListAsync (cartSpec)).FirstOrDefault ();
        }

        public async Task<IEnumerable<Cart>> GetCartsWithMatchingProductId (int productId)
        {
            EnsureArg.IsNotNull (productId, nameof (productId));
            return await _context.Carts.Where (c => c.Items.Any (i => i.CatalogItemId == productId)).ToListAsync ();
        }

        private async Task<List<Cart>> ListAsync (ISpecification<Cart> spec)
        {
            var queryableResultWithIncludes = spec.Includes
                .Aggregate (_context.Carts.AsQueryable (),
                    (current, include) => current.Include (include));
            var secondaryResult = spec.IncludeStrings
                .Aggregate (queryableResultWithIncludes,
                    (current, include) => current.Include (include));
            return await secondaryResult.Where (spec.Criteria).ToListAsync ();
        }
    }
}
