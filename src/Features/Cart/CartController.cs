using RolleiShop.Models.Entities;
using RolleiShop.Models.Entities.Order;
using RolleiShop.Models.Interfaces;
using RolleiShop.Infra.App;
using RolleiShop.Infra.Identity;
using RolleiShop.Data.Context;
using RolleiShop.Services.Interfaces;
using RolleiShop.Features.Catalog;
using RolleiShop.Infra.App.Interfaces;
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
        private readonly ICartViewModelService _cartViewModelService;
        private readonly IRepository<CatalogItem> _itemRepository;
        private readonly IOrderService _orderService;

        public CartController(
            SignInManager<ApplicationUser> signInManager,
            ILogger<CartController> logger,
            ICartService cartService,
            IOrderService orderService,
            IRepository<CatalogItem> itemRepository,
            ICartViewModelService cartViewModelService)
        {
            _logger = logger;
            _signInManager = signInManager;
            _cartService = cartService;
            _orderService = orderService;
            _itemRepository = itemRepository;
            _cartViewModelService = cartViewModelService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var cartModel = await GetCartViewModelAsync();

            return View(cartModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(Dictionary<string, int> items)
        {
            var cartViewModel = await GetCartViewModelAsync();
            await _cartService.SetQuantities(cartViewModel.Id, items);

            return View(await GetCartViewModelAsync());
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(CatalogItemViewModel productDetails)
        {
            if (productDetails?.Id == null)
            {
                return RedirectToAction("Index", "Catalog");
            }

            var catalogItem = _itemRepository.GetById (productDetails.Id);

            var cartViewModel = await GetCartViewModelAsync();

            await _cartService.AddItemToCart(cartViewModel.Id, catalogItem.Id, catalogItem.Price, 1);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            if (productId == null)
            {
                return RedirectToAction("Index", "Catalog");
            }

            var cartViewModel = await GetCartViewModelAsync();

            await _cartService.RemoveItemFromCart(cartViewModel.Id, productId);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(Dictionary<string, int> items)
        {
            var cartViewModel = await GetCartViewModelAsync();
            await _cartService.SetQuantities(cartViewModel.Id, items);
            await _orderService.CreateOrderAsync(cartViewModel.Id, new Address("123 Main St.", "Kent", "OH", "United States", "44240"));

            await _cartService.DeleteCartAsync(cartViewModel.Id);

            return View();
        }

        private async Task<CartViewModel> GetCartViewModelAsync()
        {
            if (_signInManager.IsSignedIn(User))
            {
                return await _cartViewModelService.GetOrCreateCartForUser(User.Identity.Name);
            }
            string anonymousId = GetOrSetCartCookie();
            return await _cartViewModelService.GetOrCreateCartForUser(anonymousId);
        }

        private string GetOrSetCartCookie()
        {
            if (Request.Cookies.ContainsKey("RolleiShop"))
            {
                return Request.Cookies["RolleiShop"];
            }
            string anonymousId = Guid.NewGuid().ToString();
            var cookieOptions = new CookieOptions();
            cookieOptions.Expires = DateTime.Today.AddYears(10);
            Response.Cookies.Append("RolleiShop", anonymousId, cookieOptions);
            return anonymousId;
        }
    }

    public class CartViewModel
    {
        public int Id { get; set; }
        public List<CartItemViewModel> Items { get; set; } = new List<CartItemViewModel>();
        public string BuyerId { get; set; }
        public decimal Total()
        {
            return Math.Round(Items.Sum(x => x.UnitPrice * x.Quantity), 2);
        }
    }

    public class CartItemViewModel
    {
        public int Id { get; set; }
        public int CatalogItemId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal OldUnitPrice { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; }
    }

    public class CartComponentViewModel
    {
        public int ItemsCount { get; set; }
    }

    public class ProductDetailsViewModel
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
    }
}
