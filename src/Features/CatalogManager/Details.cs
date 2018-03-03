using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RolleiShop.Data.Context;
using RolleiShop.Entities;
using RolleiShop.Infrastructure;
using RolleiShop.Infrastructure.Interfaces;

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
            private readonly ApplicationDbContext _context;
            private readonly IUrlComposer _urlComposer;

            public Handler(ApplicationDbContext context,
                IUrlComposer urlComposer)
            {
                _context = context;
                _urlComposer = urlComposer;
            }

            protected override async Task<Result<Model>> HandleCore(Query message)
            {
                var catalogItem = await SingleAsync(message.Id);

                if (catalogItem == null)
                    return Result.Fail<Model> ("Catalog Item does not exit");

                var model = new Model
                {
                    Id = catalogItem.Id,
                    Name = catalogItem.Name,
                    ImageUrl = _urlComposer.ComposeImgUrl(catalogItem.ImageUrl),
                    Description = catalogItem.Description,
                    Price = catalogItem.Price
                };

                return Result.Ok (model);
            }

            private async Task<CatalogItem> SingleAsync(int id)
            {
                return await _context.CatalogItems
                    .Include(c => c.CatalogBrand)
                    .Include(c => c.CatalogType)
                    .Where(c => c.Id == id)
                    .SingleOrDefaultAsync();
            }
        }
    }
}
