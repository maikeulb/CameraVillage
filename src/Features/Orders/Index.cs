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

namespace RolleiShop.Features.Orders
{
    public class Index
    {
        public class Query : IRequest<IEnumerable<Result>>
        {
            public string Name { get; set; }
        }

        public class Result
        {
            public int OrderNumber { get; set; }
            public DateTimeOffset OrderDate { get; set; }
            public decimal Total { get; set; }
            public string Status { get; set; }
            public Address ShippingAddress { get; set; } 
            public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

            public class OrderItem
            {
                public int ProductId { get; set; }
                public string ProductName { get; set; }
                public decimal UnitPrice { get; set; }
                public decimal Discount { get; set; }
                public int Units { get; set; }
                public string ImageUrl { get; set; }
            }

        }

        public class Handler : AsyncRequestHandler<Query, IEnumerable<Result>>
        {
            private readonly ApplicationDbContext _context;

            public Handler(ApplicationDbContext context)
            {
                _context = context;
            }

            protected override async Task<IEnumerable<Result>> HandleCore(Query message)
            {
                var orders = await ListAsync(new CustomerOrdersWithItemsSpecification(message.Name));
                return orders
                    .Select(o => new Result
                    {
                        OrderDate = o.OrderDate,
                        OrderItems = o.OrderItems?.Select(oi => new Result.OrderItem()
                        {
                            Discount = 0,
                            ImageUrl = oi.ItemOrdered.ImageUrl,
                            ProductId = oi.ItemOrdered.CatalogItemId,
                            ProductName = oi.ItemOrdered.ProductName,
                            UnitPrice = oi.UnitPrice,
                            Units = oi.Units
                        }).ToList(),
                        OrderNumber = o.Id,
                        ShippingAddress = o.ShipToAddress,
                        Status = "Pending",
                        Total = o.Total()
                    });
            }

            private  async Task<List<Order>> ListAsync(ISpecification<Order> spec)
            {
                var queryableResultWithIncludes = spec.Includes
                    .Aggregate(_context.Set<Order>().AsQueryable(),
                        (current, include) => current.Include(include));
                var secondaryResult = spec.IncludeStrings
                    .Aggregate(queryableResultWithIncludes,
                        (current, include) => current.Include(include));
                return await secondaryResult
                                .Where(spec.Criteria)
                                .ToListAsync();
            }
        }
    }
}
