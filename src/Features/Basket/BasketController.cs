using RolleiShop.Models.Entities;
using RolleiShop.Models.Interfaces;
using RolleiShop.Infra.App;
using RolleiShop.Infra.Identity;
using RolleiShop.Data.Context;
using RolleiShop.Services.Interfaces;
using RolleiShop.Infra.App.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace RolleiShop.Features.Basket
{
    public class BasketController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IBasketService _basketService;
        private readonly IBasketViewModelService _basketViewModelService;

        public BasketController(
            SignInManager<IdentityUser> signInManager,
            IBasketService basketService,
            IBasketViewModelService basketViewModelService)
        {
            _signInManager = signInManager;
            _basketService = basketService;
            _basketViewModelService = basketViewModelService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var basketModel = await GetBasketViewModelAsync();

            return View(basketModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(Dictionary<string, int> items)
        {
            var basketViewModel = await GetBasketViewModelAsync();
            await _basketService.SetQuantities(basketViewModel.Id, items);

            return View(await GetBasketViewModelAsync());
        }

        private async Task<BasketViewModel> GetBasketViewModelAsync()
        {
            return await _basketViewModelService.GetOrCreateBasketForUser(User.Identity.Name);
        }
    }

    public class BasketViewModel
    {
        public int Id { get; set; }
        public List<BasketItemViewModel> Items { get; set; } = new List<BasketItemViewModel>();
        public string BuyerId { get; set; }
        public decimal Total()
        {
            return Math.Round(Items.Sum(x => x.UnitPrice * x.Quantity), 2);
        }
    }

    public class BasketItemViewModel
    {
        public int Id { get; set; }
        public int CatalogItemId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal OldUnitPrice { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; }
    }
}
