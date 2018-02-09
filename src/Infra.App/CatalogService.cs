using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using RolleiShop.Features.Catalog;
using RolleiShop.Infra.App;
using RolleiShop.Infra.App.Interfaces;
using RolleiShop.Models.Entities;
using RolleiShop.Models.Interfaces;
using RolleiShop.Services.Interfaces;
using RolleiShop.Specifications;

namespace RolleiShop.Infra.App
{
    public class CatalogService : ICatalogService
    {
        private readonly ILogger _logger;
        private readonly IRepository<CatalogItem> _itemRepository;
        private readonly IAsyncRepository<CatalogBrand> _brandRepository;
        private readonly IAsyncRepository<CatalogType> _typeRepository;

        public CatalogService (
            ILogger<CatalogService> logger,
            IRepository<CatalogItem> itemRepository,
            IAsyncRepository<CatalogBrand> brandRepository,
            IAsyncRepository<CatalogType> typeRepository
        )
        {
            _logger = logger;
            _itemRepository = itemRepository;
            _brandRepository = brandRepository;
            _typeRepository = typeRepository;
        }

        public async Task<CatalogIndexViewModel> GetCatalogItems (int pageIndex, int itemsPage, int? brandId, int? typeId)
        {
            var filterSpecification = new CatalogFilterSpecification (brandId, typeId);
            var root = _itemRepository.List (filterSpecification);
            var totalItems = root.Count ();
            var itemsOnPage = root
                .Skip (itemsPage * pageIndex)
                .Take (itemsPage)
                .ToList ();

            var vm = new CatalogIndexViewModel ()
            {
                CatalogItems = itemsOnPage.Select (i => new CatalogItemViewModel ()
                {
                Id = i.Id,
                Name = i.Name,
                ImageUrl = i.ImageUrl,
                Price = i.Price
                }),
                Brands = await GetBrands (),
                Types = await GetTypes (),
                BrandFilterApplied = brandId ?? 0,
                TypesFilterApplied = typeId ?? 0,
                PaginationInfo = new PaginationInfoViewModel ()
                {
                ActualPage = pageIndex,
                ItemsPerPage = itemsOnPage.Count,
                TotalItems = totalItems,
                TotalPages = int.Parse (Math.Ceiling (((decimal) totalItems / itemsPage)).ToString ())
                }
            };

            foreach (var vmimg in vm.CatalogItems)
            { }
            vm.PaginationInfo.Next = (vm.PaginationInfo.ActualPage == vm.PaginationInfo.TotalPages - 1) ? "is-disabled" : "";
            vm.PaginationInfo.Previous = (vm.PaginationInfo.ActualPage == 0) ? "is-disabled" : "";

            return vm;
        }

        public CatalogDetailViewModel GetCatalogDetailItem (int catalogItemId)
        {
            _logger.LogInformation ("GetCatalogItem called.");
            var catalogItem = _itemRepository.GetById (catalogItemId);

            var vm = new CatalogDetailViewModel
            {
                Id = catalogItem.Id,
                Name = catalogItem.Name,
                ImageUrl = catalogItem.ImageUrl,
                Description = catalogItem.Description,
                Price = catalogItem.Price
            };

            return vm;
        }

        public async Task<IEnumerable<SelectListItem>> GetBrands ()
        {
            _logger.LogInformation ("GetBrands called.");
            var brands = await _brandRepository.ListAllAsync ();
            var items = new List<SelectListItem>
            {
                new SelectListItem () { Value = null, Text = "All", Selected = true }
            };
            foreach (CatalogBrand brand in brands)
            {
                items.Add (new SelectListItem () { Value = brand.Id.ToString (), Text = brand.Brand });
            }

            return items;
        }

        public async Task<IEnumerable<SelectListItem>> GetTypes ()
        {
            _logger.LogInformation ("GetTypes called.");
            var types = await _typeRepository.ListAllAsync ();
            var items = new List<SelectListItem>
            {
                new SelectListItem () { Value = null, Text = "All", Selected = true }
            };
            foreach (CatalogType type in types)
            {
                items.Add (new SelectListItem () { Value = type.Id.ToString (), Text = type.Type });
            }

            return items;
        }
    }
}