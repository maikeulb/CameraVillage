using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RolleiShop.Data.Context;
using RolleiShop.Services;
using RolleiShop.Services.Interfaces;
using RolleiShop.Identity;
using RolleiShop.Models.Entities;
using RolleiShop.Models.Interfaces;
using RolleiShop.Infra.App.Interfaces;

namespace RolleiShop.Features.CatalogManager
{
    public class CatalogManagerController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMediator _mediator;
        private readonly ApplicationDbContext _context;

        public CatalogManagerController (
            ApplicationDbContext context,
            SignInManager<ApplicationUser> signInManager,
            IMediator mediator)
        {
            _signInManager = signInManager;
            _mediator = mediator;
            _context = context;
        }

        [Authorize(Roles="Admin, DemoAdmin")]
        public async Task<IActionResult> Index(Index.Query query)
        {
            var model = await _mediator.Send(query);

            return View(model);
        }

        [Authorize(Roles="Admin, DemoAdmin")]
        public async Task<IActionResult> Details (Details.Query query)
        {
            var modelOrError = await _mediator.Send (query);

            return modelOrError.IsSuccess
                ? (IActionResult)View(modelOrError.Value)
                : (IActionResult)BadRequest(modelOrError.Error);
        }

        [Authorize(Roles="Admin, DemoAdmin")]
        public async Task<IActionResult> Create ()
        {
            await PopulateDropdownLists();

            return View ();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles="Admin")]
        public async Task<IActionResult> Create (Create.Command command)
        {
            await _mediator.Send(command);

            return RedirectToAction ("Index");
        }

        [Authorize(Roles="Admin, DemoAdmin")]
        public async Task<IActionResult> Edit (Edit.Query query)
        {
            var modelOrError = await _mediator.Send (query);

            return modelOrError.IsSuccess
                ? (IActionResult)View(modelOrError.Value)
                : (IActionResult)BadRequest(modelOrError.Error);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles="Admin")]
        public async Task<IActionResult> Edit (Edit.Command command)
        {
            var result = await _mediator.Send(command);

            return result.IsSuccess
                ? (IActionResult)RedirectToAction ("Index")
                : (IActionResult)BadRequest(result.Error);
        }

        [Authorize(Roles="Admin, DemoAdmin")]
        public async Task<IActionResult> Delete(Delete.Query query)
        {
            var modelOrError = await _mediator.Send (query);

            return modelOrError.IsSuccess
                ? (IActionResult)View(modelOrError.Value)
                : (IActionResult)BadRequest(modelOrError.Error);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles="Admin")]
        public async Task<IActionResult> Delete(Delete.Command command)
        {
            var result = await _mediator.Send(command);

            return result.IsSuccess
                ? (IActionResult)RedirectToAction ("Index")
                : (IActionResult)BadRequest(result.Error);
        }

        private async Task PopulateDropdownLists()
        {
            ViewBag.BrandId = await GetBrands();
            ViewBag.TypeId = await GetTypes();
        }

        private async Task<IEnumerable<SelectListItem>> GetBrands ()
        {
            var brands = await _context.CatalogBrands
                .AsNoTracking()
                .ToListAsync();
            var items = new List<SelectListItem>();
            foreach (CatalogBrand brand in brands)
            {
                items.Add (new SelectListItem () { Value = brand.Id.ToString (), Text = brand.Brand });
            }
            return items;
        }

        private async Task<IEnumerable<SelectListItem>> GetTypes ()
        {
            var types = await _context.CatalogTypes
                .AsNoTracking()
                .ToListAsync();
            var items = new List<SelectListItem>();
            foreach (CatalogType type in types)
            {
                items.Add (new SelectListItem () { Value = type.Id.ToString (), Text = type.Type });
            }
            return items;
        }
    }
}
