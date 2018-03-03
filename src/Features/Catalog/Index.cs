using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RolleiShop.Data.Context;
using RolleiShop.Infrastructure;
using RolleiShop.Infrastructure.Interfaces;
using RolleiShop.Entities;
using RolleiShop.Services;
using RolleiShop.Services.Interfaces;
using RolleiShop.Specifications;
using RolleiShop.Specifications.Interfaces;

namespace RolleiShop.Features.Catalog
{
    public class Index
    {
        public class Query : IRequest<Model>
        {
            public int? BrandFilterApplied { get; set; }
            public int? TypesFilterApplied { get; set; }
            public int? Page { get; set; }
        }

        public class Model
        {
            public IEnumerable<CatalogItem> CatalogItems { get; set; }
            public IEnumerable<SelectListItem> Brands { get; set; }
            public IEnumerable<SelectListItem> Types { get; set; }
            public int? BrandFilterApplied { get; set; }
            public int? TypesFilterApplied { get; set; }
            public PaginationInfoViewModel PaginationInfo { get; set; }

            public class CatalogItem
            {
                public int Id { get; set; }
                public string Name { get; set; }
                public string ImageUrl { get; set; }
                public decimal Price { get; set; }
            }

            public class PaginationInfoViewModel
            {
                public int TotalItems { get; set; }
                public int ItemsPerPage { get; set; }
                public int ActualPage { get; set; }
                public int TotalPages { get; set; }
                public string Previous { get; set; }
                public string Next { get; set; }
            }
        }

        public class Handler : AsyncRequestHandler<Query, Model>
        {
            private readonly ApplicationDbContext _context;
            private readonly IUrlComposer _urlComposer;
            private readonly IMemoryCache _cache;
            private readonly IDistributedCache _distributedCache;
            private readonly ILogger _logger;
            private static readonly string _brandsKey = "brands";
            private static readonly string _typesKey = "types";
            private static readonly string _itemsKeyTemplate = "items-{0}-{1}-{2}-{3}";
            private static readonly TimeSpan _defaultCacheDuration = TimeSpan.FromSeconds (30);

            public Handler (ApplicationDbContext context,
                IMemoryCache cache,
                ILogger<Index.Handler> logger,
                IDistributedCache distributedCache,
                IUrlComposer urlComposer)
            {
                _cache = cache;
                _logger = logger;
                _context = context;
                _urlComposer = urlComposer;
                _distributedCache = distributedCache;
            }

            protected override async Task<Model> HandleCore (Query message)
            {
                int itemsPage = 8;
                int pageNumber = message.Page ?? 0;

                return await GetCatalogItems (pageNumber, itemsPage, message.BrandFilterApplied, message.TypesFilterApplied);
            }

            private async Task<Model> GetCatalogItems (int pageIndex, int itemsPage, int? brandId, int? typeId)
            {
                string cacheKey = String.Format (_itemsKeyTemplate, pageIndex, itemsPage, brandId, typeId);
                var filterSpecification = new CatalogFilterSpecification (brandId, typeId);
                IEnumerable<Model.CatalogItem> root = await ListAsync (filterSpecification, cacheKey);
                var totalItems = root.Count ();
                var itemsOnPage = root
                    .Skip (itemsPage * pageIndex)
                    .Take (itemsPage)
                    .ToList ();
                itemsOnPage.ForEach (x =>
                {
                    x.ImageUrl = _urlComposer.ComposeImgUrl (x.ImageUrl);
                });
                var result = new Model ()
                {
                    CatalogItems = itemsOnPage,
                    Brands = await GetBrands (),
                    Types = await GetTypes (),
                    BrandFilterApplied = brandId ?? 0,
                    TypesFilterApplied = typeId ?? 0,
                    PaginationInfo = new Model.PaginationInfoViewModel ()
                    {
                        ActualPage = pageIndex,
                        ItemsPerPage = itemsOnPage.Count,
                        TotalItems = totalItems,
                        TotalPages = int.Parse (Math.Ceiling (((decimal) totalItems / itemsPage)).ToString ())
                    }
                };
                foreach (var n in result.CatalogItems) { }
                result.PaginationInfo.Next = (result.PaginationInfo.ActualPage == result.PaginationInfo.TotalPages - 1) ? "is-disabled" : "";
                result.PaginationInfo.Previous = (result.PaginationInfo.ActualPage == 0) ? "is-disabled" : "";
                return result;
            }

            private async Task<IEnumerable<SelectListItem>> GetBrands ()
            {
                return await _cache.GetOrCreateAsync (_brandsKey, async entry =>
                {
                    entry.SlidingExpiration = _defaultCacheDuration;
                    var brands = await _context
                        .CatalogBrands
                        .AsNoTracking ()
                        .ToListAsync ();
                    var items = new List<SelectListItem>
                    {
                        new SelectListItem () { Value = string.Empty, Text = "All", Selected = true }
                    };
                    foreach (CatalogBrand brand in brands)
                    {
                        items.Add (new SelectListItem () { Value = brand.Id.ToString (), Text = brand.Brand });
                    }
                    return items;
                });
            }

            private async Task<IEnumerable<SelectListItem>> GetTypes ()
            {
                return await _cache.GetOrCreateAsync (_typesKey, async entry =>
                {
                    entry.SlidingExpiration = _defaultCacheDuration;
                    var types = await _context.CatalogTypes
                        .AsNoTracking ()
                        .ToListAsync ();
                    var items = new List<SelectListItem>
                    {
                        new SelectListItem () { Value = string.Empty, Text = "All", Selected = true }
                    };
                    foreach (CatalogType type in types)
                    {
                        items.Add (new SelectListItem () { Value = type.Id.ToString (), Text = type.Type });
                    }
                    return items;
                });
            }

            private async Task<List<Model.CatalogItem>> ListAsync (ISpecification<CatalogItem> spec, string cacheKey)
            {
                var cachedCatalogItemKey = await _distributedCache.GetAsync (cacheKey);
                if (cachedCatalogItemKey == null)
                {
                    _logger.LogInformation ("Retrieve CatalogItems from Database");
                    IQueryable<CatalogItem> queryableModelWithIncludes = spec.Includes
                        .Aggregate (_context.CatalogItems.AsQueryable (),
                            (current, include) => current.Include (include));
                    IQueryable<CatalogItem> queryableCatalogItems = spec.IncludeStrings
                        .Aggregate (queryableModelWithIncludes,
                            (current, include) => current.Include (include));
                    IQueryable<CatalogItem> selectedCatalogItems = queryableCatalogItems
                        .Where (spec.Criteria);
                    IQueryable<Model.CatalogItem> catalogItems = selectedCatalogItems
                        .Select (i => new Model.CatalogItem ()
                    {
                        Id = i.Id,
                        Name = i.Name,
                        ImageUrl = i.ImageUrl,
                        Price = i.Price
                    });
                    string serializedCatalogItemsString = JsonConvert.SerializeObject (catalogItems.AsEnumerable());
                    byte[] encodedCatalogItems = Encoding.UTF8.GetBytes (serializedCatalogItemsString);
                    var cacheEntryOptions = new DistributedCacheEntryOptions ()
                        .SetSlidingExpiration (TimeSpan.FromSeconds (1000));
                    await _distributedCache.SetAsync (cacheKey, encodedCatalogItems, cacheEntryOptions);
                    return await catalogItems.ToListAsync();
                }
                else
                {
                    _logger.LogInformation ("Retrieve CatalogItems from Redis Cache");
                    byte[] encodedCatalogItems = await _distributedCache.GetAsync (cacheKey);
                    string serializedCatalogItemsString = Encoding.UTF8.GetString (encodedCatalogItems);
                    IEnumerable<Model.CatalogItem> catalogItems = JsonConvert.DeserializeObject<IEnumerable<Model.CatalogItem>> (serializedCatalogItemsString);
                    return catalogItems.ToList();
                }
            }
        }
    }
}
