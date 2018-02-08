using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RolleiShop.Services.Interfaces;
using RolleiShop.Features.Basket;
using RolleiShop.Infra.Identity;
using RolleiShop.Infra.App.Interfaces;
using RolleiShop.Services.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace RolleiShop.ViewComponents
{
    public class Basket : ViewComponent
    {
        private readonly IBasketViewModelService _basketService;
        private readonly SignInManager<IdentityUser> _signInManager;

        public Basket(IBasketViewModelService basketService,
                        SignInManager<IdentityUser> signInManager)
        {
            _basketService = basketService;
            _signInManager = signInManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string userName)
        {
            var vm = new BasketComponentViewModel();
            vm.ItemsCount = (await GetBasketViewModelAsync()).Items.Sum(i => i.Quantity);
            return View(vm);
        }

        private async Task<BasketViewModel> GetBasketViewModelAsync()
        {
            if (_signInManager.IsSignedIn(HttpContext.User))
            {
                return await _basketService.GetOrCreateBasketForUser(User.Identity.Name);
            }
            string anonymousId = GetBasketIdFromCookie();
            if (anonymousId == null) return new BasketViewModel();
            return await _basketService.GetOrCreateBasketForUser(anonymousId);
        }

        private string GetBasketIdFromCookie()
        {
            if (Request.Cookies.ContainsKey("RolleiShop"))
            {
                return Request.Cookies["RolleiShop"];
            }
            return null;
        }
    }
}
