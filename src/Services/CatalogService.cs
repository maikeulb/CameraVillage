using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using RolleiShop.Features.ManageCatalog;
using RolleiShop.Infra.App;
using RolleiShop.Infra.App.Interfaces;
using RolleiShop.Data.Context;
using RolleiShop.Models.Entities;
using RolleiShop.Models.Interfaces;
using RolleiShop.Services.Interfaces;
using RolleiShop.Specifications;

namespace RolleiShop.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _context;

        public CatalogService (
            ILogger<CatalogService> logger,
            ApplicationDbContext context
        )
        {
            _context = context;
            _logger = logger;
        }

        /* public async Task<Index.Result> GetCatalogItems (int pageIndex, int itemsPage, int? brandId, int? typeId) */
        /* { */

        /*     var filterSpecification = new CatalogFilterSpecification (brandId, typeId); */
        /*     IEnumerable<CatalogItem> root = await ListAsync (filterSpecification); */
        /*     var totalItems = root.Count (); */
        /*     var itemsOnPage = root */
        /*         .Skip (itemsPage * pageIndex) */
        /*         .Take (itemsPage) */
        /*         .ToList (); */

        /*     var vm = new Index.Result () */
        /*     { */
        /*         CatalogItems = itemsOnPage.Select (i => new CatalogItemViewModel () */
        /*         { */
        /*             Id = i.Id, */
        /*             Name = i.Name, */
        /*             ImageUrl = i.ImageUrl, */
        /*             Price = i.Price */
        /*         }), */
        /*         Brands = await GetBrands (), */
        /*         Types = await GetTypes (), */
        /*         BrandFilterApplied = brandId ?? 0, */
        /*         TypesFilterApplied = typeId ?? 0, */
        /*         PaginationInfo = new PaginationInfoViewModel () */
        /*         { */
        /*             ActualPage = pageIndex, */
        /*             ItemsPerPage = itemsOnPage.Count, */
        /*             TotalItems = totalItems, */
        /*             TotalPages = int.Parse (Math.Ceiling (((decimal) totalItems / itemsPage)).ToString ()) */
        /*         } */
        /*     }; */

        /*     foreach (var vmimg in vm.CatalogItems) */
        /*     { } */
        /*     vm.PaginationInfo.Next = (vm.PaginationInfo.ActualPage == vm.PaginationInfo.TotalPages - 1) ? "is-disabled" : ""; */
        /*     vm.PaginationInfo.Previous = (vm.PaginationInfo.ActualPage == 0) ? "is-disabled" : ""; */

        /*     return vm; */
        /* } */

        public async Task<Details.Result> GetCatalogDetailItem (int catalogItemId)
        {
            var catalogItem = await _context.Set<CatalogItem>().FindAsync(catalogItemId);

            var vm = new Details.Result
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
            IEnumerable<CatalogBrand> brands = await _context.Set<CatalogBrand>().ToListAsync();
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
            IEnumerable<CatalogType> types  = await _context.Set<CatalogType>().ToListAsync();
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

        private async Task<List<CatalogItem>> ListAsync(ISpecification<CatalogItem> spec)
        {
            var queryableResultWithIncludes = spec.Includes
                .Aggregate(_context.Set<CatalogItem>().AsQueryable(),
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
