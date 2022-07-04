using System.Globalization;
using System.Threading.Tasks;
using Artice.Core.AspNetCore;
using Artice.Core.AspNetCore.Models;
using Artice.Core.IncomingMessages;
using Artice.Vk.Configuration;
using Artice.Vk.Models;
using Artice.Vk.Models.Enum;

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

        protected override async Task<WebhookResponse> MakeResponse(Update updateObject)
        {
            var response = await base.MakeResponse(updateObject);

            if (updateObject.Type == UpdateType.Confirmation && 
                string.Equals(updateObject.GroupId.ToString(CultureInfo.InvariantCulture), _configuration.GroupId))
            {
                response.Body = _configuration.WebhookVerifyToken;
            }
            else
            {
                response.Body = "ok";
            }

            return response;
        }
    }
}