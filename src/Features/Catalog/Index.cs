using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using RolleiShop.Data.Context;
using RolleiShop.Services;
using RolleiShop.Services.Interfaces;
using RolleiShop.Models.Entities;
using RolleiShop.Models.Interfaces;
using RolleiShop.Infra.App.Interfaces;

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
            public IEnumerable<CatalogItemViewModel> CatalogItems { get; set; }
            public IEnumerable<SelectListItem> Brands { get; set; }
            public IEnumerable<SelectListItem> Types { get; set; }
            public int? BrandFilterApplied { get; set; }
            public int? TypesFilterApplied { get; set; }
            public PaginationInfoViewModel PaginationInfo { get; set; }
        }

        public class Handler : AsyncRequestHandler<Query, Result>
        {
            private readonly ICatalogService _catalogService;

            public Handler(ICatalogService catalogService)
            {
                _catalogService = catalogService;
            }

            protected override async Task<Result> HandleCore(Query message)
            {
                int itemsPage = 10;
                int pageNumber = message.Page ?? 0;

                return await _catalogService
                    .GetCatalogItems (pageNumber, itemsPage, message.BrandFilterApplied, message.TypesFilterApplied);
            }
        }
    }
}
