using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RolleiShop.Data.Context;
using RolleiShop.Services;
using RolleiShop.Services.Interfaces;
using RolleiShop.Entities;
using RolleiShop.Infrastructure.Interfaces;

namespace RolleiShop.Features.CatalogManager
{
    public class Delete
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

            public QueryHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            protected override async Task<Result<Command>> HandleCore(Query message)
            {
                var catalogItem = await SingleAsync(message.Id);

                if (catalogItem == null)
                    return Result.Fail<Command> ("Catalog Item does not exit");

                var command = new Command
                {
                    Id = catalogItem.Id,
                    Name = catalogItem.Name,
                    Brand = catalogItem.CatalogBrand.Brand,
                    Type = catalogItem.CatalogType.Type,
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
                    .SingleOrDefaultAsync(m=>m.Id == message.Id);

                if (catalogItem == null)
                    return Result.Fail<Command> ("Catalog Item does not exit");

                _context.Remove(catalogItem);
                await _context.SaveChangesAsync ();

                return Result.Ok ();
            }
        }
    }
}
