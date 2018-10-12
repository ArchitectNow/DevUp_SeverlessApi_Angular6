using System.Diagnostics;
using System.Threading.Tasks;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using ArchitectNow.Identity.Models;
using Serilog;

namespace ArchitectNow.Identity.Controllers
{
    public class HomeController : Controller
    {
        private static ILogger Logger => Log.Logger;
        private readonly IHostingEnvironment _env;
        private readonly IIdentityServerInteractionService _interaction;

        public HomeController(IHostingEnvironment env,
             IIdentityServerInteractionService interaction)
        {
            _env = env;
            _interaction = interaction;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Error(string errorId)
        {
            ViewData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            Logger.Information($"Enviroment: {_env.EnvironmentName}");

            // retrieve error details from identityserver
            var message = await _interaction.GetErrorContextAsync(errorId);
            var model = new IdentityServerErrorModel
            {
                IsDevelopment = _env.IsDevelopment(),
                IdentityServerError = message
            };
            return View(model);
        }
    }
}