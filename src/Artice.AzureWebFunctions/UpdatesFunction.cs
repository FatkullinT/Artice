using System;
using System.Threading.Tasks;
using Artice.AspNetCore.AzureWebJob;
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
        public Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "updates/{provider}")] HttpRequest req,
            string provider)
        {
            return base.ProcessUpdate(req, provider);
        }
    }
}
