using System.Globalization;
using System.Threading.Tasks;
using Artice.Core.AspNetCore;
using Artice.Core.IncomingMessages;
using Artice.Vk.Configuration;
using Artice.Vk.Models;
using Artice.Vk.Models.Enum;
using Microsoft.AspNetCore.Http;

namespace Artice.Vk.AspNetCore
{
	[HandlerRoute("/vk")]
	internal class VkWebhookRequestHandler : WebhookRequestHandler<Update>
	{
        private readonly VkProviderConfiguration _configuration;

        public VkWebhookRequestHandler(IIncomingUpdateHandler<Update> updateHandler, VkProviderConfiguration configuration) : base(updateHandler)
        {
            _configuration = configuration;
        }

        protected override async Task MakeResponse(HttpContext context, Update updateObject)
        {
            await base.MakeResponse(context, updateObject);

            if (updateObject.Type == UpdateType.Confirmation && 
                string.Equals(updateObject.GroupId.ToString(CultureInfo.InvariantCulture), _configuration.GroupId))
            {
                await context.Response.WriteAsync(_configuration.WebhookVerifyToken);
            }
            else
            {
                await context.Response.WriteAsync("ok");
            }
        }
    }
}