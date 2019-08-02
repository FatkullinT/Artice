using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Artice.Core.IncomingMessages;
using Artice.Core.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Artice.Core.AspNetCore
{
	public abstract class WebhookRequestHandler<TUpdate> : IWebhookRequestHandler
	{
		private readonly IIncomingUpdateHandler<TUpdate> _updateHandler;

		protected WebhookRequestHandler(IIncomingUpdateHandler<TUpdate> updateHandler)
		{
			_updateHandler = updateHandler;
		}

		public virtual Task<bool> CheckRequest(HttpRequest request)
		{
			return Task.FromResult(request.Method == HttpMethod.Post.Method);
		}

		public virtual async Task<IncomingMessage> HandleAsync(HttpContext context)
		{
			using (Stream receiveStream = context.Request.Body)
			{
				using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8))
				{
					var content = await readStream.ReadToEndAsync();
					var updateObject = await ConvertJsonContent(content);
                    var incomingMessage = await _updateHandler.HandleAsync(updateObject);
                    await MakeResponse(context, updateObject);
					return incomingMessage;
				}
			}
		}

		protected virtual Task<TUpdate> ConvertJsonContent(string content)
		{
			return Task.FromResult(JsonConvert.DeserializeObject<TUpdate>(content));
        }

		protected virtual Task MakeResponse(HttpContext context, TUpdate updateObject)
		{
			context.Response.StatusCode = StatusCodes.Status200OK;
			return Task.CompletedTask;
		}
	}
}