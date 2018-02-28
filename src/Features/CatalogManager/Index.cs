using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
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

namespace RolleiShop.Features.CatalogManager
{
    public class Index
    {
        public class Query : IRequest<Model>
        {
            public int? Page { get; set; }
        }

        public class Model
        {
            public IEnumerable<CatalogItem> CatalogItems { get; set; }
            public PaginationInfoViewModel PaginationInfo { get; set; }

                public class CatalogItem
                {
                    public int Id { get; set; }
                    public string Name { get; set; }
                    public string Brand { get; set; }
                    public string Type { get; set; }
                    public int AvailableStock { get; set; }
                    [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
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

            public Handler(ApplicationDbContext context)
            {
                _context = context;
            }

            protected override async Task<Model> HandleCore(Query message)
            {
                int itemsPage = 10;
                int pageNumber = message.Page ?? 0;

                return await GetCatalogItems (pageNumber, itemsPage);
            }

            private async Task<Model> GetCatalogItems (int pageIndex, int itemsPage)
            {
                IEnumerable<CatalogItem> root = await ListAsync ();
                var totalItems = root.Count ();
                var itemsOnPage = root
                    .Skip (itemsPage * pageIndex)
                    .Take (itemsPage)
                    .ToList ();

                var model = new Model ()
                {
                    CatalogItems = itemsOnPage.Select (i => new Model.CatalogItem ()
                    {
                        Id = i.Id,
                        Name = i.Name,
                        Brand = i.CatalogBrand.Brand,
                        Type = i.CatalogType.Type,
                        Price = i.Price,
                        AvailableStock = i.AvailableStock,
                    }),
                    PaginationInfo = new Model.PaginationInfoViewModel ()
                    {
                        ActualPage = pageIndex,
                        ItemsPerPage = itemsOnPage.Count,
                        TotalItems = totalItems,
                        TotalPages = int.Parse (Math.Ceiling (((decimal) totalItems / itemsPage)).ToString ())
                    }
                };

                foreach (var n in model.CatalogItems)
                { }
                model.PaginationInfo.Next = (model.PaginationInfo.ActualPage == model.PaginationInfo.TotalPages - 1) ? "is-disabled" : "";
                model.PaginationInfo.Previous = (model.PaginationInfo.ActualPage == 0) ? "is-disabled" : "";

                return model;
            }

            private async Task<List<CatalogItem>> ListAsync()
            {
                return await _context.CatalogItems
                    .AsNoTracking()
                    .Include(c => c.CatalogBrand)
                    .Include(c => c.CatalogType)
                    .ToListAsync();
            }
        }
    }
}
