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

namespace RolleiShop.Apis.Basket
{
    [Authorize]
    [Route ("api/[Controller]")]
    public class BasketApiController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBasketService _basketService;
        private readonly IBasketViewModelService _basketViewModelService;
        private readonly IOrderService _orderService;


        public BasketApiController (
            UserManager<ApplicationUser> userManager,
            IBasketService basketService,
            IOrderService orderService,
            IBasketViewModelService basketViewModelService)
        {
            _userManager = userManager;
            _basketService = basketService;
            _orderService = orderService;
            _basketViewModelService = basketViewModelService;
        }

        [HttpPost]
        public async Task<IActionResult> AddToBasket(CatalogItemViewModel productDetails)
        {
            if (productDetails?.Id == null)
            {
                return RedirectToAction("Index", "Catalog");
            }
            /* var basketViewModel = await GetBasketViewModelAsync(); */

            /* await _basketService.AddItemToBasket(basketViewModel.Id, productDetails.Id, productDetails.Price, 1); */
            return RedirectToAction("Index", "Catalog");
        }
    }
}
