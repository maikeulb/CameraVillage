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
using RolleiShop.Models.Entities;
using RolleiShop.Models.Interfaces;
using RolleiShop.Infra.App;
using RolleiShop.Infra.App.Interfaces;
using RolleiShop.Specifications;

namespace RolleiShop.Apis.Cart
{
    public class RemoveFromCart
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
                await _cartService.RemoveItemFromCart (message.Id, catalogItem.Id);
            }
        }
    }
}