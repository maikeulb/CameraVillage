using System;
using System.IO;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RolleiShop.Data.Context;
using RolleiShop.Services;
using RolleiShop.Services.Interfaces;
using RolleiShop.Models.Entities;
using RolleiShop.Models.Interfaces;
using RolleiShop.Infra.App;
using RolleiShop.Infra.App.Interfaces;
using RolleiShop.Specifications;

namespace RolleiShop.Features.CatalogManager
{
    public class Edit
    {
        public class Query : IRequest<Command>
        {
            public int Id { get; set; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(m => m.Id).NotNull();
            }
        }

        public class QueryHandler : AsyncRequestHandler<Query, Command>
        {
            private readonly ApplicationDbContext _context;
            private readonly IUrlComposer _urlComposer;

            public QueryHandler(ApplicationDbContext context,
                IUrlComposer urlComposer)
            {
                _context = context;
                _urlComposer = urlComposer;
            }

            protected override async Task<Command> HandleCore(Query message)
            {
                var catalogItem = await SingleAsync(message.Id);

                return new Command
                {
                    Id = catalogItem.Id,
                    Name = catalogItem.Name,
                    Brand = catalogItem.CatalogBrand.Brand,
                    Type = catalogItem.CatalogType.Type,
                    Description = catalogItem.Description,
                    Stock = catalogItem.AvailableStock,
                    Price = catalogItem.Price,
                    ImageUrl = _urlComposer.ComposeImgUrl(catalogItem.ImageUrl)
                };
            }

            private async Task<CatalogItem> SingleAsync(int id)
            {
                return await _context.Set<CatalogItem>()
                    .Include(c => c.CatalogBrand)
                    .Include(c => c.CatalogType)
                    .SingleOrDefaultAsync(c => c.Id == id);
            }
        }

        public class Command : IRequest
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Brand { get; set; }
            public string Type { get; set; }
            public string Description { get; set; }
            public int Stock { get; set; }
            public decimal Price { get; set; }
            public string ImageUrl { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(m => m.Stock).NotNull();
                RuleFor(m => m.Price).NotNull();
                RuleFor(m => m.Name).NotNull();
                RuleFor(m => m.Description).NotNull();
            }
        }

        public class CommandHandler : AsyncRequestHandler<Command>
        {
            private readonly ApplicationDbContext _context;

            public CommandHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            protected override async Task HandleCore(Command message)
            {
                var catalogItem = _context.Set<CatalogItem>().Find(message.Id);
                catalogItem.UpdateDetails (message);
                _context.CatalogItems.Update (catalogItem);
                await _context.SaveChangesAsync ();
            }
        }
    }
}
