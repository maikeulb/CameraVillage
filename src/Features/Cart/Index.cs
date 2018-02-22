using System;
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
using RolleiShop.Infra.App;
using RolleiShop.Infra.App.Interfaces;
using RolleiShop.Specifications;

namespace RolleiShop.Features.Cart
{
    public class Index
    {
        public class Command : IRequest
        {
            public Dictionary<string, int> Items { get; set; }
            public int CartId { get; set; }
        }

        public class Handler : AsyncRequestHandler<Command>
        {
            private readonly ICartService _cartService;

            public Handler(ICartService cartService)
            {
                _cartService = cartService;
            }

            protected override async Task HandleCore(Command message)
            {
                await _cartService.SetQuantities(message.CartId, message.Items);
            }
        }
    }
}
