using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RolleiShop.Data.Context;
using RolleiShop.Features.Cart;
using RolleiShop.Features.Catalog;
using RolleiShop.Services;
using RolleiShop.Services.Interfaces;
using RolleiShop.Infra.Identity;
using RolleiShop.Models.Entities;
using RolleiShop.Models.Interfaces;

namespace RolleiShop.Apis.Cart
{
    [Route ("api/[Controller]")]
    public class CartController : Controller
    {
        private readonly ILogger _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICartService _cartService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ICartViewModelService _cartViewModelService;
        private readonly IOrderService _orderService;
        private readonly ApplicationDbContext _context;

        public CartController (
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            ILogger<CartController> logger,
            ICartService cartService,
            IOrderService orderService,
            ICartViewModelService cartViewModelService)
        {
            _userManager = userManager;
            _logger = logger;
            _signInManager = signInManager;
            _cartService = cartService;
            _orderService = orderService;
            _cartViewModelService = cartViewModelService;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart ([FromBody] ProductViewModel product)
        {
            var catalogItem =  await _context.Set<CatalogItem>().FindAsync(product.ProductId);
            var cartViewModel = await GetCartViewModelAsync ();
            await _cartService.AddItemToCart (cartViewModel.Id, catalogItem.Id, catalogItem.Price, 1);

            return Ok ();
        }

        private async Task<GetCart.Result> GetCartViewModelAsync ()
        {
            if (_signInManager.IsSignedIn (User))
            {
                return await _cartViewModelService.GetOrCreateCartForUser (User.Identity.Name);
            }
            string anonymousId = GetOrSetCartCookie ();
            return await _cartViewModelService.GetOrCreateCartForUser (anonymousId);
        }

        private string GetOrSetCartCookie ()
        {
            if (Request.Cookies.ContainsKey ("RolleiShop"))
            {
                return Request.Cookies["RolleiShop"];
            }
            string anonymousId = Guid.NewGuid ().ToString ();
            var cookieOptions = new CookieOptions ();
            cookieOptions.Expires = DateTime.Today.AddYears (10);
            Response.Cookies.Append ("RolleiShop", anonymousId, cookieOptions);
            return anonymousId;
        }

    }

    public class ProductViewModel
    {
        public int ProductId { get; set; }
    }
}
