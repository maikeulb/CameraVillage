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
using RolleiShop.Infra.App.Interfaces;

namespace RolleiShop.Features.ManageCatalog
{
    public class Delete
    {
        public class Query : IRequest<Command>
        {
            public int Id { get; set; }
        }

        public class QueryHandler : AsyncRequestHandler<Query, Command>
        {
            private readonly ApplicationDbContext _context;

            public QueryHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            protected override async Task<Command> HandleCore(Query message)
            {
                var catalogItem = await _context.Set<CatalogItem>().FindAsync(message.Id);
                var model = new Command
                {
                    Id = catalogItem.Id,
                    Name = catalogItem.Name,
                    BrandId = catalogItem.CatalogBrandId,
                    TypeId = catalogItem.CatalogTypeId,
                };
                //handle model not found
                return model;
            }
        }

        public class Command : IRequest
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int BrandId { get; set; }
            public int TypeId { get; set; }
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
                var catalogItem = await _context.CatalogItems
                    .AsNoTracking()
                    .SingleOrDefaultAsync(m=>m.Id == message.Id);
                _context.Remove(catalogItem);
                await _context.SaveChangesAsync ();
            }
        }
    }
}
