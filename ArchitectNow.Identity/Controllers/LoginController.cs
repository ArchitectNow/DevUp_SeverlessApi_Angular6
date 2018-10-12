using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ArchitectNow.Identity.Models;
using ArchitectNow.Identity.Services;
using ArchitectNow.Mongo.Identity;

namespace ArchitectNow.Identity.Controllers
{
    [Authorize]
    public class AuthController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger _logger;

        private readonly IIdentityServerInteractionService _interaction;
        private readonly AccountService _account;

        public AuthController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ILogger<AuthController> logger,
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IHttpContextAccessor httpContextAccessor,
            IAuthenticationSchemeProvider schemeProvider
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;

            _interaction = interaction;
            _account = new AccountService(_interaction, httpContextAccessor, schemeProvider, clientStore);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, 
                    lockoutOnFailure: false, isPersistent: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return RedirectToLocal(returnUrl);
                }
               
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }

            _logger.LogInformation("Oh snap!!! Failed Login");
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Logout(string logoutId)
        {
            // build a model so the logout page knows what to display
            var vm = await _account.BuildLogoutViewModelAsync(logoutId);

            if (vm.ShowLogoutPrompt == false)
            {
                // if the request for logout was properly authenticated from IdentityServer, then
                // we don't need to show the prompt and can just log the user out directly.
                return await Logout(vm);
            }

            return View(vm);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(LogoutInputModel model)
        {
            var vm = await _account.BuildLoggedOutViewModelAsync(model.LogoutId);

            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");

            // check if we need to trigger sign-out at an upstream identity provider
            if (vm.TriggerExternalSignout)
            {
                // build a return URL so the upstream provider will redirect back
                // to us after the user has logged out. this allows us to then
                // complete our single sign-out processing.
                string url = Url.Action("Logout", new { logoutId = vm.LogoutId });

                // this triggers a redirect to the external provider for sign-out
                // hack: try/catch to handle social providers that throw
                return SignOut(new AuthenticationProperties { RedirectUri = url }, vm.ExternalAuthenticationScheme);
            }

            return View("LoggedOut", vm);
        }
        
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Claims = new List<AppUserClaim>
                    {
                        new AppUserClaim(new Claim(JwtClaimTypes.Name, model.Email)),
                        new AppUserClaim(new Claim(JwtClaimTypes.GivenName, model.FirstName)),
                        new AppUserClaim(new Claim(JwtClaimTypes.FamilyName, model.LastName)),
                        new AppUserClaim(new Claim(JwtClaimTypes.Email, model.Email)),
                        new AppUserClaim(new Claim(JwtClaimTypes.Picture, model.ImageUrl ?? string.Empty)),
                        new AppUserClaim(new Claim(JwtClaimTypes.Subject, "http://localhost:4000"))
                    },
                    Roles = new List<string> { "Administrator" }
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=532713
                    // Send an email with this link
                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //var callbackUrl = Url.Action(nameof(ConfirmEmail), "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                    //await _emailSender.SendEmailAsync(model.Email, "Confirm your account",
                    //    $"Please confirm your account by clicking this link: <a href='{callbackUrl}'>link</a>");
                    _logger.LogInformation(3, "User created a new account with password.");
                    return RedirectToLocal(returnUrl);
                }

                AddErrors(result);
            }

            return View(model);
        }


        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }
        
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}