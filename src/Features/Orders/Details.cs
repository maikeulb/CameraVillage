using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RolleiShop.Data.Context;
using RolleiShop.Services;
using RolleiShop.Services.Interfaces;
using RolleiShop.Entities;
using RolleiShop.Infrastructure.Interfaces;

namespace RolleiShop.Features.Orders
{
    public class Details
    {
        public class Query : IRequest<Model>
        {
            public int Id { get; set; }
        }

        public class Model
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
                [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
                public decimal UnitPrice { get; set; }
                public int Units { get; set; }
                public string ImageUrl { get; set; }
            }
        }

        public class Handler : AsyncRequestHandler<Query, Model>
        {
            private readonly ApplicationDbContext _context;

            public Handler(ApplicationDbContext context)
            {
                _context = context;
            }

            protected override async Task<Model> HandleCore(Query message)
            {
                var order = await FirstAsync();
                return new Model()
                {
                    OrderDate = order.OrderDate,
                    OrderItems = order.OrderItems.Select(oi => new Model.OrderItem()
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

            private async Task<Order> FirstAsync()
            {
                return await _context.Orders
                    .Include(c => c.OrderItems)
                    .Include("OrderItems.ItemOrdered")
                    .FirstOrDefaultAsync();
            }
        }
    }
}
