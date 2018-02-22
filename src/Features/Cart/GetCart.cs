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
using RolleiShop.Specifications;
using RolleiShop.Models.Entities;
using RolleiShop.Models.Interfaces;
using RolleiShop.Infra.App.Interfaces;

namespace RolleiShop.Features.Cart
{
    public class GetCart
    {
        public class Query : IRequest<Result>
        {
            public string Name { get; set; }
        }

        public class Result
        {
            public int Id { get; set; }
            public List<CartItem> Items { get; set; } = new List<CartItem>();
            public string BuyerId { get; set; }
            public decimal Total()
            {
                return Math.Round(Items.Sum(x => x.UnitPrice * x.Quantity), 2);
            }

            public class CartItem
            {
                public int Id { get; set; }
                public int CatalogItemId { get; set; }
                public string ProductName { get; set; }
                public decimal UnitPrice { get; set; }
                public decimal OldUnitPrice { get; set; }
                public int Quantity { get; set; }
                public string ImageUrl { get; set; }
            }
        }

        public class Handler : AsyncRequestHandler<Query, Result>
        {
            private readonly ICartViewModelService _cartViewModelService;

            public Handler(ICartViewModelService cartViewModelService)
            {
                _cartViewModelService = cartViewModelService;
            }

            protected override async Task<Result> HandleCore(Query message)
            {
                return await _cartViewModelService.GetOrCreateCartForUser(message.Name);
            }
        }
    }
}
