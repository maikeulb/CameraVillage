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
        private readonly IAsyncRepository<Cart> _cartRepository;
        private readonly IRepository<CatalogItem> _itemRepository;

        public CartService(IAsyncRepository<Cart> cartRepository,
            IRepository<CatalogItem> itemRepository)
        {
            _cartRepository = cartRepository;
            _itemRepository = itemRepository;
        }

        public async Task AddItemToCart(int cartId, int catalogItemId, decimal price, int quantity)
        {
            var cart = await _cartRepository.GetByIdAsync(cartId);

            cart.AddItem(catalogItemId, price, quantity);

            await _cartRepository.UpdateAsync(cart);
        }

        public async Task RemoveItemFromCart(int cartId, int catalogItemId)
        {
            var cart = await _cartRepository.GetByIdAsync(cartId);

            cart.RemoveItem(catalogItemId);

            await _cartRepository.UpdateAsync(cart);
        }

        public async Task DeleteCartAsync(int cartId)
        {
            var cart = await _cartRepository.GetByIdAsync(cartId);

            await _cartRepository.DeleteAsync(cart);
        }

        public async Task<int> GetCartItemCountAsync(string userName)
        {
            var cartSpec = new CartWithItemsSpecification(userName);
            var cart = (await _cartRepository.ListAsync(cartSpec)).FirstOrDefault();
            if (cart == null)
                return 0;
            int count = cart.Items.Sum(i => i.Quantity);
            return count;
        }

        public async Task SetQuantities(int cartId, Dictionary<string, int> quantities)
        {
            var cart = await _cartRepository.GetByIdAsync(cartId);
            foreach (var item in cart.Items)
            {
                if (quantities.TryGetValue(item.Id.ToString(), out var quantity))
                {
                    item.Quantity = quantity;
                }
            }
            await _cartRepository.UpdateAsync(cart);
        }
    }
}
