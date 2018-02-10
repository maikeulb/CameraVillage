using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RolleiShop.Models.Entities;
using RolleiShop.Models.Entities.Order;
using RolleiShop.Models.Interfaces;
using RolleiShop.Infra.App;
using RolleiShop.Infra.Identity;
using RolleiShop.Data.Context;
using RolleiShop.Services.Interfaces;
using RolleiShop.Features.Catalog;
using RolleiShop.Infra.App.Interfaces;

namespace RolleiShop.Apis.Cart
{
    [Authorize]
    [Route ("api/[Controller]")]
    public class CartApiController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICartService _cartService;
        private readonly ICartViewModelService _cartViewModelService;
        private readonly IOrderService _orderService;


        public CartApiController (
            UserManager<ApplicationUser> userManager,
            ICartService cartService,
            IOrderService orderService,
            ICartViewModelService cartViewModelService)
        {
            _userManager = userManager;
            _cartService = cartService;
            _orderService = orderService;
            _cartViewModelService = cartViewModelService;
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(CatalogItemViewModel productDetails)
        {
            if (productDetails?.Id == null)
            {
                return RedirectToAction("Index", "Catalog");
            }
            /* var cartViewModel = await GetCartViewModelAsync(); */

            /* await _cartService.AddItemToCart(cartViewModel.Id, productDetails.Id, productDetails.Price, 1); */
            return RedirectToAction("Index", "Catalog");
        }
    }
}
