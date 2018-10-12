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
    public static class DeleteWodFunction
    {
        [FunctionName("DeleteWodFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "wods/{id}")]HttpRequestMessage req, 
            ILogger log, string id,
            ExecutionContext context)        
        {
            context.Initialize();
            if (!await req.Authorize(roles: "wods", log: log)) 
                return new UnauthorizedResult();

            log.LogInformation($"Delete wod {id}");
            
            return new NoContentResult();
        }
    }
}