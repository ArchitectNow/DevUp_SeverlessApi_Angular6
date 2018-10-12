using System.Net.Http;
using System.Threading.Tasks;
using ArchitectNow.ApiFunctions.Auth;
using ArchitectNow.ApiFunctions.Shared;
using ArchitectNow.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace ArchitectNow.ApiFunctions.Functions.Wods
{
    public static class CreateWodFunction
    {
        [FunctionName("CreateWodFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "wods")]HttpRequestMessage req, 
            ExecutionContext context, ILogger log)
        {
            context.Initialize();
            if (!await req.Authorize(roles: "wods", log: log)) 
                return new UnauthorizedResult();

            var postData = await req.Content.ReadAsAsync<Wod>();
            
            return new OkResult();
        }
    }
}
