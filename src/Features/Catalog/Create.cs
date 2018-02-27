using System;
using System.IO;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RolleiShop.Data.Context;
using RolleiShop.Services;
using RolleiShop.Services.Interfaces;
using RolleiShop.Models.Entities;
using RolleiShop.Models.Interfaces;
using RolleiShop.Infra.App;
using RolleiShop.Infra.App.Interfaces;
using RolleiShop.Specifications;

namespace RolleiShop.Features.Catalog
{
    public class Create
    {
        public class Command : IRequest
        {
            public int Id { get; set; }
            public int CatalogTypeId { get; set; }
            public int CatalogBrandId { get; set; }
            public int AvailableStock { get; set; }
            public decimal Price { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public IFormFile ImageUpload { get; set; }
            public string ImageName { get; set; }
            public string ImageUrl { get; set; }
        }

        public class Handler : AsyncRequestHandler<Command>
        {
            private readonly ICartService _cartService;
            private readonly IOrderService _orderService;
            private readonly ApplicationDbContext _context;
            private readonly IHostingEnvironment _environment;

            public Handler(ICartService cartService, 
                    IHostingEnvironment environment,
                    IOrderService orderService,
                    ApplicationDbContext context)
            {
                _cartService = cartService;
                _orderService = orderService;
                _environment = environment;
                _context = context;
            }

            protected override async Task HandleCore(Command message)
            {
                var uploadPath = Path.Combine (_environment.WebRootPath, "images/products"); 
                var ImageName = ContentDispositionHeaderValue.Parse (message.ImageUpload.ContentDisposition).FileName.Trim ('"');
                using (var fileStream = new FileStream (Path.Combine (uploadPath, message.ImageUpload.FileName), FileMode.Create))
                {
                    await message.ImageUpload.CopyToAsync (fileStream);
                    message.ImageUrl = "http://images/products" + message.ImageName;
                }
                var item = CatalogItem.Create (
                    message.CatalogTypeId,
                    message.CatalogBrandId,
                    message.AvailableStock,
                    message.Price,
                    message.Name,
                    message.Description,
                    message.ImageName,
                    message.ImageUrl
                );
                _context.CatalogItems.Add (item);
                await _context.SaveChangesAsync ();
            }
        }
    }
}
