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
    public class Checkout
    {
        public class Command : IRequest
        {
            public Dictionary<string, int> Items { get; set; }
            public int CartId { get; set; }
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
                await _cartService.SetQuantities(message.CartId, message.Items);
                await _orderService.CreateOrderAsync(message.CartId, new Address("123 Main St.", "Kent", "OH", "United States", "44240"));
                await _cartService.DeleteCartAsync(message.CartId);
            }
        }
    }
}
