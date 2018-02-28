using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using RolleiShop.Data.Context;
using RolleiShop.Services;
using RolleiShop.Services.Interfaces;
using RolleiShop.Models.Entities;
using RolleiShop.Models.Interfaces;
using RolleiShop.Infra.App;
using RolleiShop.Infra.App.Interfaces;

namespace RolleiShop.Features.CatalogManager
{
    public class Details
    {
        public class Query : IRequest<Result<Model>>
        {
            public int Id { get; set; }
        }

        public class Model
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string ImageUrl { get; set; }
            public string Description { get; set; }
            [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
            public decimal Price { get; set; }
        }

        public class Handler : AsyncRequestHandler<Query, Result<Model>>
        {
            private readonly ICatalogService _catalogService;
            private readonly IUrlComposer _urlComposer;

            public Handler(ICatalogService catalogService,
                IUrlComposer urlComposer)
            {
                _catalogService = catalogService;
                _urlComposer = urlComposer;
            }

            protected override async Task<Result<Model>> HandleCore(Query message)
            {
                var model =  await _catalogService.GetCatalogDetailItem (message.Id);

                if (model == null)
                    return Result.Fail<Model> ("Catalog Item Details does not exit");

                model.ImageUrl = _urlComposer.ComposeImgUrl(model.ImageUrl);

                return Result.Ok (model);
            }
        }
    }
}
