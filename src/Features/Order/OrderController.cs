using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RolleiShop.Data.Context;
using RolleiShop.Models.Entities;
using RolleiShop.Models.Interfaces;
using RolleiShop.Specifications;

namespace RolleiShop.Features.Orders
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(
            ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> Index()
        {
            var orders = await ListAsync(new CustomerOrdersWithItemsSpecification(User.Identity.Name));

            var viewModel = orders
                .Select(o => new OrderViewModel()
                {
                    OrderDate = o.OrderDate,
                    OrderItems = o.OrderItems?.Select(oi => new OrderItemViewModel()
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
            return View(viewModel);
        }

        public async Task<IActionResult> Detail(int orderId)
        {

            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .Include("OrderItems.ItemOrdered")
                .FirstOrDefaultAsync();

            var viewModel = new OrderViewModel()
            {
                OrderDate = order.OrderDate,
                OrderItems = order.OrderItems.Select(oi => new OrderItemViewModel()
                {
                    Discount = 0,
                    ImageUrl = oi.ItemOrdered.ImageUrl,
                    ProductId = oi.ItemOrdered.CatalogItemId,
                    ProductName = oi.ItemOrdered.ProductName,
                    UnitPrice = oi.UnitPrice,
                    Units = oi.Units
                }).ToList(),
                OrderNumber = order.Id,
                ShippingAddress = order.ShipToAddress,
                Status = "Pending",
                Total = order.Total()
            };
            return View(viewModel);
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

    public class OrderItemViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public int Units { get; set; }
        public string ImageUrl { get; set; }
    }

    public class OrderViewModel
    {
        public int OrderNumber { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; }

        public Address ShippingAddress { get; set; } 
        public List<OrderItemViewModel> OrderItems { get; set; } = new List<OrderItemViewModel>();

    }
}
