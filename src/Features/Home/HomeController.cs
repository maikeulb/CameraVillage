using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;

namespace RolleiShop.Features.Home
{
    [ResponseCache(Duration = 30)]
    public class HomeController : Controller
    {
        private readonly IStringLocalizer<HomeController> _localizer;

        public HomeController(IStringLocalizer<HomeController> localizer)
        {
            _localizer = localizer;
        }

        [HttpGet("Index")]
        [HttpGet("")]
        public IActionResult Index()
        {
            ViewData["Message1"] = _localizer["This is an MVC application demonstrating a basic ecommerce shop with in ASP.NET Core 2.0, EF Core 2.0, Identity 2.0, Stripe, and a slew of libraries. The backend follows a DDD/CQRS-esque Architecture powered by MediatR. I built this using the superb Microsoft sample projects, tutorials, and documentation, along with varous online resources."];
            ViewData["Message2"] = _localizer["The name Rollei, refers to the German camera manufacturer, makers of the famous Rolleiflex cameras. Rollei happens to be my favorite camera brand, having owned several Rollei 35's and Rolleiflex TLRs."];
            ViewData["Message3"] = _localizer["The map points to Map Camera loked in Shinjyuko, Tokyo. Map Camera is one of the finest camera stores for new and used camera gear."];
            ViewData["Title"] = _localizer["Welcome to RolleiShop"];

            return View();
        }


        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
