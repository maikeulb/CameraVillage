using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RolleiShop.Data.Context;
using RolleiShop.Services;
using RolleiShop.Services.Interfaces;
using RolleiShop.Models.Entities;
using RolleiShop.Models.Interfaces;
using RolleiShop.Infra.App;
using RolleiShop.Infra.App.Interfaces;
using RolleiShop.Specifications;

namespace RolleiShop.Features.Catalog
{
    public class Index
    {
        public class Query : IRequest<Result>
        {
            public int? BrandFilterApplied { get; set; } 
            public int? TypesFilterApplied { get; set; } 
            public int? Page { get; set; }
        }

        public class Result
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

        public class Handler : AsyncRequestHandler<Query, Result>
        {
            private readonly ApplicationDbContext _context;
            private readonly IUrlComposer _urlComposer;

            public Handler(ApplicationDbContext context,
                IUrlComposer urlComposer)
            {
                _context = context;
                _urlComposer = urlComposer;
            }

            protected override async Task<Result> HandleCore(Query message)
            {
                int itemsPage = 10;
                int pageNumber = message.Page ?? 0;

                return await GetCatalogItems (pageNumber, itemsPage, message.BrandFilterApplied, message.TypesFilterApplied);
            }

            private async Task<Result> GetCatalogItems (int pageIndex, int itemsPage, int? brandId, int? typeId)
            {
                var filterSpecification = new CatalogFilterSpecification (brandId, typeId);
                IEnumerable<CatalogItem> root = await ListAsync (filterSpecification);
                var totalItems = root.Count ();
                var itemsOnPage = root
                    .Skip (itemsPage * pageIndex)
                    .Take (itemsPage)
                    .ToList ();

                itemsOnPage.ForEach(x =>
                {
                    x.ImageUrl = _urlComposer.ComposeImgUrl(x.ImageUrl);
                });

                var result = new Result ()
                {
                    CatalogItems = itemsOnPage.Select (i => new Result.CatalogItem ()
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
                    PaginationInfo = new Result.PaginationInfoViewModel ()
                    {
                        ActualPage = pageIndex,
                        ItemsPerPage = itemsOnPage.Count,
                        TotalItems = totalItems,
                        TotalPages = int.Parse (Math.Ceiling (((decimal) totalItems / itemsPage)).ToString ())
                    }
                };

                foreach (var n in result.CatalogItems)
                { }
                result.PaginationInfo.Next = (result.PaginationInfo.ActualPage == result.PaginationInfo.TotalPages - 1) ? "is-disabled" : "";
                result.PaginationInfo.Previous = (result.PaginationInfo.ActualPage == 0) ? "is-disabled" : "";

                return result;
            }

            private  async Task<IEnumerable<SelectListItem>> GetBrands ()
            {
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

            private async Task<IEnumerable<SelectListItem>> GetTypes ()
            {
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
}
