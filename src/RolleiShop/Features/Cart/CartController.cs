using RolleiShop.Identity;
using RolleiShop.Services.Interfaces;
using RolleiShop.ViewModels;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using RolleiShop.Notifications;

namespace RolleiShop.Features.Cart
{
    public class CartController : Controller
    {
        private readonly ILogger _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ICartViewModelService _cartViewModelService;
        private readonly IMediator _mediator;

        public CartController(
            SignInManager<ApplicationUser> signInManager,
            IMediator mediator,
            ILogger<CartController> logger,
            ICartViewModelService cartViewModelService)
        {
            _logger = logger;
            _signInManager = signInManager;
            _mediator = mediator;
            _cartViewModelService = cartViewModelService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var cartModel = await GetCartViewModelAsync();

            return View(cartModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(Index.Command command)
        {
            var cartViewModel = await GetCartViewModelAsync();
            command.Id = cartViewModel.Id;
            await _mediator.Send(command);

            return View(await GetCartViewModelAsync());
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Checkout(Checkout.Command command)
        {
            var cartViewModel = await GetCartViewModelAsync();
            command.Id = cartViewModel.Id;
            await _mediator.Send(command);

            await _mediator.Publish(new CustomerCheckoutNotification(User.Identity.Name));

            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ClearCart(ClearCart.Command command)
        {
            var cartViewModel = await GetCartViewModelAsync();
            command.Id = cartViewModel.Id;
            await _mediator.Send(command);

            return RedirectToAction ("Index");
        }

        [Authorize]
        public async Task<IActionResult> Charge(Charge.Query query)
        {
            var cartViewModel = await GetCartViewModelAsync();
            query.StripeTotal = cartViewModel.StripeTotal();
            await _mediator.Send(query);

            return View();
        }

        private async Task<CartViewModel> GetCartViewModelAsync ()
        {
            if (_signInManager.IsSignedIn (User))
                return await _cartViewModelService.GetOrCreateCartForUser (User.Identity.Name);

            string anonymousId = GetOrSetCartCookie ();

            return await _cartViewModelService.GetOrCreateCartForUser (anonymousId);
        }

        private string GetOrSetCartCookie()
        {
            if (Request.Cookies.ContainsKey("RolleiShop"))
                return Request.Cookies["RolleiShop"];

            string anonymousId = Guid.NewGuid().ToString();
            var cookieOptions = new CookieOptions();
            cookieOptions.Expires = DateTime.Today.AddYears(10);
            Response.Cookies.Append("RolleiShop", anonymousId, cookieOptions);
            return anonymousId;
        }
    }
}
