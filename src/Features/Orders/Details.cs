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
using RolleiShop.Infra.App.Interfaces;

namespace RolleiShop.Features.Orders
{
    public class Details
    {
        public class Query : IRequest<Result>
        {
            public int Id { get; set; }
        }

        public class Result
        {
            public int OrderNumber { get; set; }
            public DateTimeOffset OrderDate { get; set; }
            public decimal Total { get; set; }
            public string Status { get; set; }
            public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

            public class OrderItem
            {
                public int ProductId { get; set; }
                public string ProductName { get; set; }
                public decimal UnitPrice { get; set; }
                public int Units { get; set; }
                public string ImageUrl { get; set; }
            }
        }

        public class Handler : AsyncRequestHandler<Query, Result>
        {
            private readonly ApplicationDbContext _context;

            public Handler(ApplicationDbContext context)
            {
                _context = context;
            }

            protected override async Task<Result> HandleCore(Query message)
            {
                var order = await _context.Orders
                    .Include(o => o.OrderItems)
                    .Include("OrderItems.ItemOrdered")
                    .FirstOrDefaultAsync();
                return new Result()
                {
                    OrderDate = order.OrderDate,
                    OrderItems = order.OrderItems.Select(oi => new Result.OrderItem()
                    {
                        ImageUrl = oi.ItemOrdered.ImageUrl,
                        ProductId = oi.ItemOrdered.CatalogItemId,
                        ProductName = oi.ItemOrdered.ProductName,
                        UnitPrice = oi.UnitPrice,
                        Units = oi.Units
                    }).ToList(),
                    OrderNumber = order.Id,
                    Status = "Pending",
                    Total = order.Total()
                };
            }
        }
    }
}
