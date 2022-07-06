using System;
using System.Threading.Tasks;
using Artice.AspNetCore.AzureWebJob.Functions.Http;
using Artice.Core.AspNetCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace Artice.AzureWebFunctions
{
    public class UpdatesFunction : UpdatesFunctionBase
    {
        public UpdatesFunction(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        [FunctionName("Updates")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "updates/{provider}")] HttpRequest req,
            string provider)
        {
            var webHookResult = await base.ProcessUpdate(req, provider);
            return GetActionResult(webHookResult);
        }

        private static IActionResult GetActionResult(WebhookResponse response)
        {
            return new ObjectResult(response.Body)
            {
                StatusCode = response.StatusCode
            };
        }
    }
}
