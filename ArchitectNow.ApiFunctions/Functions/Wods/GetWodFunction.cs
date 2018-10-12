using System.Net.Http;
using System.Threading.Tasks;
using ArchitectNow.ApiFunctions.Auth;
using ArchitectNow.ApiFunctions.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace ArchitectNow.ApiFunctions.Functions.Wods
{
    public static class GetWodFunction
    {
        [FunctionName("GetWodFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "wods/{id}")]HttpRequestMessage req, 
            ILogger log, string id,
            ExecutionContext context)
        {
            context.Initialize();
            if (!await req.Authorize(roles: "wods, wods.readonly", log: log)) 
                return new UnauthorizedResult();
            
            var wodRepo = Application.Repositories.WodsRepository;
            var wod = await wodRepo.FirsteOrDefaultAsync(w => w.Id == ObjectId.Parse(id));
            return wod == null ? new JsonResult(null) : new JsonResult(wod);
        }
    }
}