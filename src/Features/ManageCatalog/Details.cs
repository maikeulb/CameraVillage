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
using RolleiShop.Infra.App;
using RolleiShop.Infra.App.Interfaces;

namespace RolleiShop.Features.ManageCatalog
{
    public class Details
    {
        public class Query : IRequest<Result>
        {
            public int Id { get; set; }
        }

        public class Result
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string ImageUrl { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
        }

        public class Handler : AsyncRequestHandler<Query, Result>
        {
            private readonly ICatalogService _catalogService;
            private readonly IUrlComposer _urlComposer;

            public Handler(ICatalogService catalogService,
                IUrlComposer urlComposer)
            {
                _catalogService = catalogService;
                _urlComposer = urlComposer;
            }

            protected override async Task<Result> HandleCore(Query message)
            {
                Result result =  await _catalogService.GetCatalogDetailItem (message.Id);
                result.ImageUrl  = _urlComposer.ComposeImgUrl(result.ImageUrl);
                return result;
            }
        }
    }
}
