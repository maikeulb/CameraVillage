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

namespace RolleiShop.Features.ManageCatalog
{
    public class Index
    {
        public class Query : IRequest<Result>
        {
            public int? Page { get; set; }
        }

        public class Result
        {
            public IEnumerable<CatalogItem> CatalogItems { get; set; }
            public PaginationInfoViewModel PaginationInfo { get; set; }

                public class CatalogItem
                {
                    public int Id { get; set; }
                    public string Name { get; set; }
                    public int BrandId { get; set; }
                    public int TypeId { get; set; }
                    public int AvailableStock { get; set; }
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

            public Handler(ApplicationDbContext context)
            {
                _context = context;
            }

            protected override async Task<Result> HandleCore(Query message)
            {
                int itemsPage = 10;
                int pageNumber = message.Page ?? 0;

                return await GetCatalogItems (pageNumber, itemsPage);
            }

            private async Task<Result> GetCatalogItems (int pageIndex, int itemsPage)
            {
                IEnumerable<CatalogItem> root = await ListAsync ();
                var totalItems = root.Count ();
                var itemsOnPage = root
                    .Skip (itemsPage * pageIndex)
                    .Take (itemsPage)
                    .ToList ();

                var result = new Result ()
                {
                    CatalogItems = itemsOnPage.Select (i => new Result.CatalogItem ()
                    {
                        Id = i.Id,
                        Name = i.Name,
                        BrandId = i.CatalogBrandId,
                        TypeId = i.CatalogTypeId,
                        Price = i.Price,
                        AvailableStock = i.AvailableStock,
                    }),
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

            public async Task<List<CatalogItem>> ListAsync()
            {
                return await _context.Set<CatalogItem>().ToListAsync();
            }
        }

    }
}
