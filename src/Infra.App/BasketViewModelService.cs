using RolleiShop.Specifications;
using RolleiShop.Models.Entities;
using RolleiShop.Models.Interfaces;
using RolleiShop.Features.Basket;
using RolleiShop.Infra.App.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RolleiShop.Infra.App
{
    public class BasketViewModelService : IBasketViewModelService
    {
        private readonly ILogger _logger;
        private readonly IAsyncRepository<Basket> _basketRepository;
        private readonly IRepository<CatalogItem> _itemRepository;

        public BasketViewModelService(
            ILogger<BasketViewModelService> logger,
            IAsyncRepository<Basket> basketRepository,
            IRepository<CatalogItem> itemRepository)
        {
            _logger = logger;
            _basketRepository = basketRepository;
            _itemRepository = itemRepository;
        }

        public async Task<BasketViewModel> GetOrCreateBasketForUser(string userName)
        {
            var basketSpec = new BasketWithItemsSpecification(userName);
            var basket = (await _basketRepository.ListAsync(basketSpec)).FirstOrDefault();

            if(basket == null)
            {
                return await CreateBasketForUser(userName);
            }
            return CreateViewModelFromBasket(basket);
        }

        private BasketViewModel CreateViewModelFromBasket(Basket basket)
        {
            var viewModel = new BasketViewModel();

            viewModel.Id = basket.Id;
            viewModel.BuyerId = basket.BuyerId;
            viewModel.Items = basket.Items.Select(i =>
            {
                var itemModel = new BasketItemViewModel()
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

        private async Task<BasketViewModel> CreateBasketForUser(string userId)
        {
            var basket = new Basket() { BuyerId = userId };
            await _basketRepository.AddAsync(basket);

            return new BasketViewModel()
            {
                BuyerId = basket.BuyerId,
                Id = basket.Id,
                Items = new List<BasketItemViewModel>()
            };
        }
    }
}
