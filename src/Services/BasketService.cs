using RolleiShop.Services.Interfaces;
using RolleiShop.Models.Entities;
using RolleiShop.Models.Interfaces;
using RolleiShop.Infra.App;
using RolleiShop.Infra.App.Interfaces;
using RolleiShop.Data.Context;
using RolleiShop.Specifications;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;

namespace RolleiShop.Services
{
    public class BasketService : IBasketService
    {
        private readonly IAsyncRepository<Basket> _basketRepository;
        private readonly IRepository<CatalogItem> _itemRepository;

        public BasketService(IAsyncRepository<Basket> basketRepository,
            IRepository<CatalogItem> itemRepository)
        {
            _basketRepository = basketRepository;
            _itemRepository = itemRepository;
        }

        public async Task AddItemToBasket(int basketId, int catalogItemId, decimal price, int quantity)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteBasketAsync(int basketId)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetBasketItemCountAsync(string userName)
        {
            throw new NotImplementedException();
        }

        public async Task SetQuantities(int basketId, Dictionary<string, int> quantities)
        {
            var basket = await _basketRepository.GetByIdAsync(basketId);
            foreach (var item in basket.Items)
            {
                if (quantities.TryGetValue(item.Id.ToString(), out var quantity))
                {
                    item.Quantity = quantity;
                }
            }
            await _basketRepository.UpdateAsync(basket);
        }

        public async Task TransferBasketAsync(string anonymousId, string userName)
        {
            throw new NotImplementedException();
        }
    }
}
