using System.Threading.Tasks;
using Artice.Core.AspNetCore.Models;
using Microsoft.AspNetCore.Http;

namespace Artice.Core.AspNetCore
{
	public interface IWebhookRequestHandler
	{
		Task<bool> CheckRequest(HttpRequest request);

		Task<WebhookProcessingResult> HandleAsync(HttpRequest request);
	}
}