using System;
using System.IO;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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

namespace RolleiShop.Features.ManageCatalog
{
    public class Edit
    {
        public class Query : IRequest<Command>
        {
            public int? Id { get; set; }
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
                    AvailableStock = catalogItem.AvailableStock,
                    Price = catalogItem.Price,
                    Description = catalogItem.Description,
                };
                return model;
            }
        }

        public class Command : IRequest
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public int AvailableStock { get; set; }
            public decimal Price { get; set; }
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
