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
    public static class UpdateWodFunction
    {
        [FunctionName("UpdateWodFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "wods/{id}")]HttpRequestMessage req, 
            ILogger log, string id,
            ExecutionContext context)        
        {
            context.Initialize();
            if (!await req.Authorize(roles: "wods", log: log)) 
                return new UnauthorizedResult();

            log.LogInformation($"Updating wod {id}");
            var postData = await req.Content.ReadAsAsync<Wod>();
            
            return new OkResult();
        }
    }
}
