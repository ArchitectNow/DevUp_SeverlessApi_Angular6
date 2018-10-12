using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ArchitectNow.ApiFunctions.Auth;
using ArchitectNow.ApiFunctions.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace ArchitectNow.ApiFunctions.Functions.Wods
{
    public static class TodaysWodFunction
    {
        [FunctionName("TodaysWodFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "wods/today")]HttpRequestMessage req, 
            ILogger log,
            ExecutionContext context)
        {
            context.Initialize();
            if (!await req.Authorize(roles: "wods, wods.readonly", log: log)) 
                return new UnauthorizedResult();
            
            var wodRepo = Application.Repositories.WodsRepository;
            var wods = await wodRepo.ToListAsync();
            var wod = wods.FirstOrDefault();
            if (wod == null)
                return new JsonResult(null);
            
            wod.WodDate = DateTimeOffset.Now;
            return new JsonResult(wod);
        }
    }
}