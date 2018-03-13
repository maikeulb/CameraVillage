using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;
using MailKit.Net.Smtp;
using MimeKit;
using MailKit;
using NETCore.MailKit.Core;

namespace RolleiShop.Features.Home
{
    [ResponseCache(Duration = 30)]
    public class HomeController : Controller
    {
        private readonly IStringLocalizer<HomeController> _localizer;
        private readonly IEmailService _emailService;

        public HomeController(IStringLocalizer<HomeController> localizer,
                              IEmailService emailService)
        {
            _localizer = localizer;
            _emailService = emailService;
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

        [HttpGet("SendEmail")]
        public IActionResult SendEmail()
        {
            ViewData["Message"] = "ASP.NET Core mvc send email example";

            string name = "Moike";
            string BodyContent = $@"<h1>{name} ASP.NET Core was previously called ASP.NET 5</h1> 
                <p>It was renamed in January 2016.</p><p>It supports cross-platform frameworks ( Windows, Linux, Mac )
                for building modern cloud-based internet-connected applications like IOT, web apps, and mobile back-end.</p>";

            _emailService.Send("maikeulbgithub@gmail.com", "ASP.NET Core mvc send email example", BodyContent, true);

            return RedirectToAction ("Index");
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
