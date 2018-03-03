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
using RolleiShop.Entities;
using RolleiShop.Infrastructure;
using RolleiShop.Infrastructure.Interfaces;

namespace RolleiShop.Features.CatalogManager
{
    public class Index
    {
        public class Query : IRequest<Model>
        {
            public int? Page { get; set; }
            public string SortOrder { get; set; }
        }

        public class Model
        {
            public IEnumerable<CatalogItem> CatalogItems { get; set; }
            public PaginationInfoViewModel PaginationInfo { get; set; }
            public string NameSortParm { get; set; }
            public string BrandSortParm { get; set; }
            public string TypeSortParm { get; set; }
            public string CurrentSort { get; set; }

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

                var catalogItems = _context.CatalogItems
                    .Include(c => c.CatalogBrand)
                    .Include(c => c.CatalogType)
                    .Select (i => new Model.CatalogItem ()
                    {
                        Id = i.Id,
                        Name = i.Name,
                        Brand = i.CatalogBrand.Brand,
                        Type = i.CatalogType.Type,
                        Price = i.Price,
                        AvailableStock = i.AvailableStock,
                    });

                switch (message.SortOrder)
                {
                    case "name_desc":
                        catalogItems = catalogItems.OrderByDescending(s => s.Name);
                        break;
                    case "Brand":
                        catalogItems = catalogItems.OrderBy(s => s.Brand);
                        break;
                    case "brand_desc":
                        catalogItems = catalogItems.OrderBy(s => s.Brand);
                        break;
                    case "Type":
                        catalogItems = catalogItems.OrderByDescending(s => s.Type);
                        break;
                    case "type_desc":
                        catalogItems = catalogItems.OrderByDescending(s => s.Type);
                        break;
                    default: 
                        catalogItems = catalogItems.OrderBy(s => s.Name);
                        break;
                }

                var root = await catalogItems.ToListAsync();

                var totalItems = root.Count ();
                var itemsOnPage = root
                    .Skip (itemsPage * pageNumber)
                    .Take (itemsPage)
                    .ToList ();

                var model = new Model ()
                {
                    CurrentSort = message.SortOrder,
                    NameSortParm = String.IsNullOrEmpty(message.SortOrder) ? "name_desc" : "",
                    BrandSortParm = message.SortOrder == "Brand" ? "brand_desc" : "Brand",
                    TypeSortParm = message.SortOrder == "Type" ? "type_desc" : "Type",

                    CatalogItems = itemsOnPage,

                    PaginationInfo = new Model.PaginationInfoViewModel ()
                    {
                        ActualPage = pageNumber,
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
        }
    }
}
