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

namespace RolleiShop.Features.Catalog
{
    public class CatalogController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMediator _mediator;

        public CatalogController (
            SignInManager<ApplicationUser> signInManager,
            IMediator mediator)
        {
            _signInManager = signInManager;
            _mediator = mediator;
        }

        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> Index(Index.Query query)
        {
            var model = await _mediator.Send(query);

            return View(model);
        }

        /* public async Task<IActionResult> Details (Details.Query query) */
        /* { */
        /*     var model = await _mediator.Send(query); */
        /*     return View(model); */
        /* } */

        /* [Authorize(Roles="Admin")] */
        /* public IActionResult Create () */
        /* { */
        /*     return View (); */
        /* } */

        /* [HttpPost] */
        /* [Authorize(Roles="Admin")] */
        /* public async Task<IActionResult> Create (Create.Command command) */
        /* { */
        /*     await _mediator.Send(command); */
        /*     return RedirectToAction ("Create"); */
        /* } */

        /* [Authorize(Roles="Admin")] */
        /* public async Task<IActionResult> Edit (Edit.Query query) */
        /* { */
        /*     if (query.Id == null) */
        /*         return NotFound (); */
        /*     var model = await _mediator.Send(query); */
        /*     return View(model); */
        /* } */

        /* [HttpPost] */
        /* [Authorize(Roles="Admin")] */
        /* public async Task<IActionResult> Edit (Edit.Command command) */
        /* { */
        /*     await _mediator.Send(command); */
        /*     return RedirectToAction ("Index"); */
        /* } */

        [HttpGet ("Error")]
        public IActionResult Error ()
        {
            return View ();
        }
    }
}
