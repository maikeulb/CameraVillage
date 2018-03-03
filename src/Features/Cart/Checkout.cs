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

namespace RolleiShop.Features.Cart
{
    public class Checkout
    {
        public class Command : IRequest
        {
            public Dictionary<string, int> Items { get; set; }
            public int Id { get; set; }
        }

        public class Handler : AsyncRequestHandler<Command>
        {
            private readonly ICartService _cartService;
            private readonly IOrderService _orderService;

            public Handler(ICartService cartService, 
                    IOrderService orderService)
            {
                _cartService = cartService;
                _orderService = orderService;
            }

            protected override async Task HandleCore(Command message)
            {
                await _cartService.SetQuantities(message.Id, message.Items);
                await _orderService.CreateOrderAsync(message.Id);
                await _cartService.DeleteCartAsync(message.Id);
            }
        }
    }
}
