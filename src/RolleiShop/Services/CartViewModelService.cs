using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RolleiShop.Data.Context;
using RolleiShop.Entities;
using RolleiShop.Infrastructure.Interfaces;
using RolleiShop.Services.Interfaces;
using RolleiShop.Specifications;
using RolleiShop.Specifications.Interfaces;
using RolleiShop.ViewModels;

namespace RolleiShop.Services
{
    public class CartViewModelService : ICartViewModelService
    {
        private readonly ILogger _logger;
        private readonly IUrlComposer _urlComposer;
        private readonly ApplicationDbContext _context;

        public CartViewModelService (ApplicationDbContext context,
            ILogger<CartViewModelService> logger,
            IUrlComposer urlComposer)
        {
            _context = context;
            _logger = logger;
            _urlComposer = urlComposer;
        }

        public async Task<CartViewModel> GetOrCreateCartForUser (string userName)
        {
            var cartSpec = new CartWithItemsSpecification (userName);
            var cart = (await ListAsync (cartSpec)).FirstOrDefault ();

            if (cart == null)
                return await CreateCartForUser (userName);

            return CreateViewModelFromCart (cart);
        }

        private CartViewModel CreateViewModelFromCart (Cart cart)
        {
            var viewModel = new CartViewModel ();

            viewModel.Id = cart.Id;
            viewModel.BuyerId = cart.BuyerId;
            viewModel.Items = cart.Items.Select (i =>
            {
                var itemModel = new CartViewModel.CartItem ()
                {
                Id = i.Id,
                UnitPrice = i.UnitPrice,
                Quantity = i.Quantity,
                CatalogItemId = i.CatalogItemId
                };
                var item = _context.CatalogItems.Find (i.CatalogItemId);
                itemModel.ImageUrl = _urlComposer.ComposeImgUrl (item.ImageUrl);
                itemModel.ProductName = item.Name;
                return itemModel;
            }).ToList ();

            return viewModel;
        }

        private async Task<CartViewModel> CreateCartForUser (string userId)
        {
            var cart = Cart.Create (userId);
            _context.Carts.Add (cart);
            await _context.SaveChangesAsync ();

            return new CartViewModel ()
            {
                BuyerId = cart.BuyerId,
                    Id = cart.Id,
                    Items = new List<CartViewModel.CartItem> ()
            };
        }

        private async Task<List<Cart>> ListAsync (ISpecification<Cart> spec)
        {
            var queryableResultWithIncludes = spec.Includes
                .Aggregate (_context.Carts.AsQueryable (),
                    (current, include) => current.Include (include));
            var secondaryResult = spec.IncludeStrings
                .Aggregate (queryableResultWithIncludes,
                    (current, include) => current.Include (include));
            return await secondaryResult
                .Where (spec.Criteria)
                .ToListAsync ();
        }
    }
}