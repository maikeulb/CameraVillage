using System.Threading.Tasks;
using FluentValidation;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RolleiShop.Data.Context;
using RolleiShop.Entities;
using RolleiShop.Infrastructure.Interfaces;

namespace RolleiShop.Features.CatalogManager
{
    public class Edit
    {
        public class Query : IRequest<Result<Command>>
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

        public class QueryHandler : AsyncRequestHandler<Query, Result<Command>>
        {
            private readonly ApplicationDbContext _context;
            private readonly IUrlComposer _urlComposer;

            public QueryHandler(ApplicationDbContext context,
                IUrlComposer urlComposer)
            {
                _context = context;
                _urlComposer = urlComposer;
            }

            protected override async Task<Result<Command>> HandleCore(Query message)
            {
                var catalogItem = await SingleAsync(message.Id);

                if (catalogItem == null)
                    return Result.Fail<Command> ("Catalog Item does not exit");

                var command =  new Command
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

                return Result.Ok (command);
            }

            private async Task<CatalogItem> SingleAsync(int id)
            {
                return await _context.CatalogItems
                    .Include(c => c.CatalogBrand)
                    .Include(c => c.CatalogType)
                    .SingleOrDefaultAsync(c => c.Id == id);
            }
        }

        public class Command : IRequest<Result>
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

        public class CommandHandler : AsyncRequestHandler<Command, Result>
        {
            private readonly ApplicationDbContext _context;

            public CommandHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            protected override async Task<Result> HandleCore(Command message)
            {
                var catalogItem = await _context.CatalogItems
                    .AsNoTracking()
                    .SingleOrDefaultAsync(m=>m.Id == message.Id);

                if (catalogItem == null)
                    return Result.Fail<Command> ("Catalog Item does not exit");

                catalogItem.UpdateDetails (message);
                _context.CatalogItems.Update (catalogItem);
                await _context.SaveChangesAsync ();

                return Result.Ok ();
            }
        }
    }
}
