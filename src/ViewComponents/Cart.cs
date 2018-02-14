using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RolleiShop.Features.Cart;
using RolleiShop.Infra.Identity;
using RolleiShop.Services.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace RolleiShop.ViewComponents
{
    public class Cart : ViewComponent
    {
        private readonly ICartViewModelService _cartService;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public Cart(ICartViewModelService cartService,
                        SignInManager<ApplicationUser> signInManager)
        {
            _cartService = cartService;
            _signInManager = signInManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string userName)
        {
            var vm = new CartComponentViewModel();
            vm.ItemsCount = (await GetCartViewModelAsync()).Items.Sum(i => i.Quantity);
            return View(vm);
        }

        private async Task<CartViewModel> GetCartViewModelAsync()
        {
            if (_signInManager.IsSignedIn(HttpContext.User))
            {
                return await _cartService.GetOrCreateCartForUser(User.Identity.Name);
            }
            string anonymousId = GetCartIdFromCookie();
            if (anonymousId == null) return new CartViewModel();
            return await _cartService.GetOrCreateCartForUser(anonymousId);
        }

        private string GetCartIdFromCookie()
        {
            if (Request.Cookies.ContainsKey("RolleiShop"))
            {
                return Request.Cookies["RolleiShop"];
            }
            return null;
        }
    }
}
