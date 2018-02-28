using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
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
    [Authorize(Roles="Admin")]
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

        public async Task<IActionResult> Create ()
        {
            await PopulateDropdownLists();

            return View ();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create (Create.Command command)
        {
            await _mediator.Send(command);

            return RedirectToAction ("Index");
        }

        public async Task<IActionResult> Edit (Edit.Query query)
        {
            if (query.Id == null)
                return NotFound ();

            var model = await _mediator.Send(query);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit (Edit.Command command)
        {
            await _mediator.Send(command);

            return RedirectToAction ("Index");
        }

        public async Task<IActionResult> Delete(Delete.Query query)
        {
            var model = await _mediator.Send(query);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Delete.Command command)
        {
            await _mediator.Send(command);

            return RedirectToAction ("Index");
        }

        private async Task PopulateDropdownLists()
        {
            ViewBag.BrandId = await GetBrands();
            ViewBag.TypeId = await GetTypes();
        }

        private async Task<IEnumerable<SelectListItem>> GetBrands ()
        {
            IEnumerable<CatalogBrand> brands = await _context.Set<CatalogBrand>().ToListAsync();
            var items = new List<SelectListItem>();
            foreach (CatalogBrand brand in brands)
            {
                items.Add (new SelectListItem () { Value = brand.Id.ToString (), Text = brand.Brand });
            }
            return items;
        }

        private async Task<IEnumerable<SelectListItem>> GetTypes ()
        {
            IEnumerable<CatalogType> types  = await _context.Set<CatalogType>().ToListAsync();
            var items = new List<SelectListItem>();
            foreach (CatalogType type in types)
            {
                items.Add (new SelectListItem () { Value = type.Id.ToString (), Text = type.Type });
            }
            return items;
        }
    }
}
