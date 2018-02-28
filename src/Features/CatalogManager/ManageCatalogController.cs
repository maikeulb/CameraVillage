using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
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

        public CatalogManagerController (
            SignInManager<ApplicationUser> signInManager,
            IMediator mediator)
        {
            _signInManager = signInManager;
            _mediator = mediator;
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

        public IActionResult Create ()
        {
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
    }
}
