using System.Collections.Generic;
using MediatR;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using RolleiShop.Data.Context;
using RolleiShop.Services;
using RolleiShop.Services.Interfaces;
using RolleiShop.Infra.Identity;
using RolleiShop.Models.Entities;
using RolleiShop.Models.Interfaces;
using RolleiShop.Infra.App.Interfaces;

namespace RolleiShop.Features.Catalog
{
    public class CatalogController : Controller
    {
        private readonly ICatalogService _catalogService;
        private readonly IHostingEnvironment _environment;
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMediator _mediator;

        public CatalogController (
            ICatalogService catalogService,
            SignInManager<ApplicationUser> signInManager,
            IHostingEnvironment environment,
            IMediator mediator,
            ApplicationDbContext context)
        {
            _catalogService = catalogService;
            _signInManager = signInManager;
            _environment = environment;
            _context = context;
            _mediator = mediator;
        }

        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> Index(Index.Query query)
        {
            var model = await _mediator.Send(query);

            return View(model);
        }

        public async Task<IActionResult> Details (Details.Query query)
        {
            var model = await _mediator.Send(query);

            return View(model);
        }

        public IActionResult Create ()
        {
            return View ();
        }

        [HttpPost]
        public async Task<IActionResult> Create (CatalogItemFormViewModel product)
        {
            var uploadPath = Path.Combine (_environment.WebRootPath, "images/products"); 

            var ImageName = ContentDispositionHeaderValue.Parse (product.ImageUpload.ContentDisposition).FileName.Trim ('"');

            using (var fileStream = new FileStream (Path.Combine (uploadPath, product.ImageUpload.FileName), FileMode.Create))
            {
                await product.ImageUpload.CopyToAsync (fileStream);
                product.ImageUrl = "http://images/products" + product.ImageName;
            }

            var item = CatalogItem.Create (
                product.CatalogTypeId,
                product.CatalogBrandId,
                product.AvailableStock,
                product.Price,
                product.Name,
                product.Description,
                product.ImageName,
                product.ImageUrl
            );

            _context.CatalogItems.Add (item);
            await _context.SaveChangesAsync ();

            return RedirectToAction ("Create");
        }

        public IActionResult Edit (int? id)
        {
            if (id == null)
                return NotFound ();

            var catalogItem = _context.Set<CatalogItem>().Find(id.Value);

            var vm = new CatalogItemEditFormViewModel
            {
                Id = catalogItem.Id,
                AvailableStock = catalogItem.AvailableStock,
                Price = catalogItem.Price,
                Description = catalogItem.Description,
            };

            if (vm == null)
                return NotFound ();

            return View (vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit (CatalogItemEditFormViewModel product)
        {
            var catalogItem = _context.Set<CatalogItem>().Find(product.Id);

            if (catalogItem == null)
                return NotFound (new { Message = $"Item with id {product.Id} not found." });

            catalogItem.UpdateDetails (product);
            _context.CatalogItems.Update (catalogItem);
            await _context.SaveChangesAsync ();

            return RedirectToAction ("Index");
        }

        [HttpGet ("Error")]
        public IActionResult Error ()
        {
            return View ();
        }
    }

    /* public class CatalogItemViewModel */
    /* { */
    /*     public int Id { get; set; } */
    /*     public string Name { get; set; } */
    /*     public string ImageUrl { get; set; } */
    /*     public decimal Price { get; set; } */
    /* } */

    public class PaginationInfoViewModel
    {
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int ActualPage { get; set; }
        public int TotalPages { get; set; }
        public string Previous { get; set; }
        public string Next { get; set; }
    }

    public class CatalogItemFormViewModel
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

    public class CatalogItemEditFormViewModel
    {
        public int Id { get; set; }
        public int AvailableStock { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
