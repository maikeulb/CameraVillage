using Microsoft.EntityFrameworkCore;
using RolleiShop.Specifications;
using RolleiShop.Models.Entities;
using RolleiShop.Models.Interfaces;
using RolleiShop.Features.Cart;
using RolleiShop.Services.Interfaces;
using RolleiShop.Data.Context;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RolleiShop.Services
{
    public class CartViewModelService : ICartViewModelService
    {
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _context;

        public CartViewModelService(ApplicationDbContext context,
            ILogger<CartViewModelService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<CartViewModel> GetOrCreateCartForUser(string userName)
        {
            var cartSpec = new CartWithItemsSpecification(userName);
            var cart = (await ListAsync(cartSpec)).FirstOrDefault();

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

                var item = _context.Set<CatalogItem>().Find(i.CatalogItemId);

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

            _context.Set<Cart>().Add(cart);
            await _context.SaveChangesAsync();

            return new CartViewModel()
            {
                BuyerId = cart.BuyerId,
                Id = cart.Id,
                Items = new List<CartItemViewModel>()
            };
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