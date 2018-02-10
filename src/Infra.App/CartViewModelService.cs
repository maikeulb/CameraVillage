using RolleiShop.Specifications;
using RolleiShop.Models.Entities;
using RolleiShop.Models.Interfaces;
using RolleiShop.Features.Cart;
using RolleiShop.Infra.App.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RolleiShop.Infra.App
{
    public class CartViewModelService : ICartViewModelService
    {
        private readonly ILogger _logger;
        private readonly IAsyncRepository<Cart> _cartRepository;
        private readonly IRepository<CatalogItem> _itemRepository;

        public CartViewModelService(
            ILogger<CartViewModelService> logger,
            IAsyncRepository<Cart> cartRepository,
            IRepository<CatalogItem> itemRepository)
        {
            _logger = logger;
            _cartRepository = cartRepository;
            _itemRepository = itemRepository;
        }

        public async Task<CartViewModel> GetOrCreateCartForUser(string userName)
        {
            var cartSpec = new CartWithItemsSpecification(userName);
            var cart = (await _cartRepository.ListAsync(cartSpec)).FirstOrDefault();

            if(cart == null)
            {
                return await CreateCartForUser(userName);
            }
            return CreateViewModelFromCart(cart);
        }

        private CartViewModel CreateViewModelFromCart(Cart cart)
        {
            var viewModel = new CartViewModel();

            viewModel.Id = cart.Id;
            viewModel.BuyerId = cart.BuyerId;
            viewModel.Items = cart.Items.Select(i =>
            {
                var itemModel = new CartItemViewModel()
                {
                    Id = i.Id,
                    UnitPrice = i.UnitPrice,
                    Quantity = i.Quantity,
                    CatalogItemId = i.CatalogItemId
                };

                var item = _itemRepository.GetById(i.CatalogItemId);

                itemModel.ImageUrl = item.ImageUrl;
                itemModel.ProductName = item.Name;
                return itemModel;
            })
                            .ToList();
            return viewModel;
        }

        private async Task<CartViewModel> CreateCartForUser(string userId)
        {
            var cart = new Cart() { BuyerId = userId };
            await _cartRepository.AddAsync(cart);

            return new CartViewModel()
            {
                BuyerId = cart.BuyerId,
                Id = cart.Id,
                Items = new List<CartItemViewModel>()
            };
        }
    }
}
