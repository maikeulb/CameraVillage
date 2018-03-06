using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RolleiShop.Identity;

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
        public async Task<IActionResult> Index (Index.Query query)
        {
            var model = await _mediator.Send (query);

            return View (model);
        }

        [HttpGet ("Error")]
        public IActionResult Error ()
        {
            return View ();
        }
    }
}
