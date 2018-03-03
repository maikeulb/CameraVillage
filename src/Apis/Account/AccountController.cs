using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RolleiShop.Data.Context;
using RolleiShop.ViewModels;
using RolleiShop.Services;
using RolleiShop.Services.Interfaces;
using RolleiShop.Entities;
using RolleiShop.Features.Account.AccountViewModels;
using RolleiShop.Features.Home;
using RolleiShop.Infrastructure;
using RolleiShop.Identity;

namespace RolleiShop.Apis.Account
{
    [Route ("api/account")]
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICartService _cartService;
        private readonly ILogger _logger;

        public AccountController (
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ICartService cartService,
            ILogger<AccountController> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _cartService = cartService;
            _logger = logger;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login ([FromBody]LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                ViewData["ReturnUrl"] = returnUrl;

                var result = await _signInManager.PasswordSignInAsync (model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    string anonymousCartId = Request.Cookies["RolleiShop"];
                    if (!String.IsNullOrEmpty(anonymousCartId))
                    {
                        await _cartService.TransferCartAsync(anonymousCartId, model.Email);
                        Response.Cookies.Delete("RolleiShop");
                    }
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError (string.Empty, "Invalid login attempt.");
                    return View (model);
                }
            }

            return View (model);
        }

        private IActionResult RedirectToLocal (string returnUrl)
        {
            if (Url.IsLocalUrl (returnUrl))
            {
                return Redirect (returnUrl);
            }
            else
            {
                return RedirectToAction (nameof (HomeController.Index), "Home");
            }
        }
    }
}
