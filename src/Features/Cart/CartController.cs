using RolleiShop.Models.Entities;
using RolleiShop.Models.Interfaces;
using RolleiShop.Infra.App;
using RolleiShop.Identity;
using RolleiShop.Data.Context;
using RolleiShop.Services.Interfaces;
using RolleiShop.ViewModels;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace RolleiShop.Features.Cart
{
    public class CartController : Controller
    {
        private readonly ILogger _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ICartService _cartService;
        private readonly ApplicationDbContext _context;
        private readonly ICartViewModelService _cartViewModelService;
        private readonly IOrderService _orderService;
        private readonly IMediator _mediator;

        public CartController(
            SignInManager<ApplicationUser> signInManager,
            IMediator mediator,
            ILogger<CartController> logger,
            ICartService cartService,
            IOrderService orderService,
            ApplicationDbContext context,
            ICartViewModelService cartViewModelService)
        {
            _logger = logger;
            _signInManager = signInManager;
            _cartService = cartService;
            _orderService = orderService;
            _mediator = mediator;
            _context = context;
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
            command.CartId = cartViewModel.Id;
            await _mediator.Send(command);

            return View(await GetCartViewModelAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(Checkout.Command command)
        {
            var cartViewModel = await GetCartViewModelAsync();
            command.CartId = cartViewModel.Id;
            await _mediator.Send(command);

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
