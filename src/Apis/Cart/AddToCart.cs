using System;
using System.IO;
using System.Collections.Generic;
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
using RolleiShop.Specifications;

namespace RolleiShop.Apis.Cart
{
    public class AddToCart
    {
        public class Command : IRequest
        {
            public int ProductId { get; set; }
            public int Id { get; set; }
        }

        public class Handler : AsyncRequestHandler<Command>
        {
            private readonly ICartService _cartService;
            private readonly ApplicationDbContext _context;

            public Handler(ICartService cartService, 
                    ApplicationDbContext context)
            {
                _cartService = cartService;
                _context = context;
            }

            protected override async Task HandleCore(Command message)
            {
                var catalogItem =  await _context.CatalogItems.FindAsync(message.ProductId);
                await _cartService.AddItemToCart (message.Id, catalogItem.Id, catalogItem.Price, 1);
            }
        }
    }
}
