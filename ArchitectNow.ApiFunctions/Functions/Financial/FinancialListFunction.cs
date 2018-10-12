using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ArchitectNow.ApiFunctions.Auth;
using ArchitectNow.ApiFunctions.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace ArchitectNow.ApiFunctions.Functions.Financial
{
    public static class FinancialListFunction
    {
        [FunctionName("FinancialListFunction")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "financial")]
            HttpRequestMessage req, 
            ExecutionContext context,
            ILogger log)
        {
            //Uncomment to secure function.
            // context.Initialize();
            // if (!await req.Authorize(roles: "financial, financial.readonly", log: log)) 
            //     return new UnauthorizedResult();

            var financials = new List<string>();
            financials.Add("1000.56");
            financials.Add("2000.99");
            financials.Add("433.34");
            return new JsonResult(financials);
        }
    }
}
