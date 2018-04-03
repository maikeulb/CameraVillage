using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RolleiShop.Data.Context;
using RolleiShop.Specifications;
using RolleiShop.Specifications.Interfaces;
using RolleiShop.Entities;

namespace RolleiShop.Features.Orders
{
    public class Index
    {
        public class Query : IRequest<IEnumerable<Model>>
        {
            public string Name { get; set; }
        }

        public class Model
        {
            public int OrderNumber { get; set; }
            [DisplayFormat(DataFormatString = "{0:d}")]
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

        public class Handler : AsyncRequestHandler<Query, IEnumerable<Model>>
        {
            private readonly ApplicationDbContext _context;

            public Handler(ApplicationDbContext context)
            {
                _context = context;
            }

            protected override async Task<IEnumerable<Model>> HandleCore(Query message)
            {
                var orders = await ListAsync(new CustomerOrdersWithItemsSpecification(message.Name));
                return orders
                    .Select(o => new Model
                    {
                        OrderDate = o.OrderDate,
                        OrderItems = o.OrderItems?.Select(oi => new Model.OrderItem()
                        {
                            ImageUrl = oi.ItemOrdered.ImageUrl,
                            ProductId = oi.ItemOrdered.CatalogItemId,
                            ProductName = oi.ItemOrdered.ProductName,
                            UnitPrice = oi.UnitPrice,
                            Units = oi.Units
                        }).ToList(),
                        OrderNumber = o.Id,
                        Status = "Pending",
                        Total = o.Total()
                    });
            }

            private async Task<List<Order>> ListAsync(ISpecification<Order> spec)
            {
                var queryableModelWithIncludes = spec.Includes
                    .Aggregate(_context.Orders.AsQueryable(),
                        (current, include) => current.Include(include));
                var secondaryModel = spec.IncludeStrings
                    .Aggregate(queryableModelWithIncludes,
                        (current, include) => current.Include(include));
                return await secondaryModel.Where(spec.Criteria)
                    .AsNoTracking()
                    .ToListAsync();
            }
        }
    }
}
