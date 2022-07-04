using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Artice.Core.AspNetCore.Models;
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

		public async Task<WebhookProcessingResult> HandleAsync(HttpRequest request)
		{
			using (Stream receiveStream = request.Body)
			{
				using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8))
				{
					var content = await readStream.ReadToEndAsync();
					var updateObject = await ConvertJsonContent(content);
                    var incomingMessage = await _updateHandler.HandleAsync(updateObject);
                    var response = await MakeResponse(updateObject);
					var processingResult = new WebhookProcessingResult
					{
						IncomingMessage = incomingMessage,
						Response = response
					};

					return processingResult;
				}
			}
		}

        protected virtual Task<TUpdate> ConvertJsonContent(string content)
		{
			return Task.FromResult(JsonConvert.DeserializeObject<TUpdate>(content));
        }

		protected virtual Task<WebhookResponse> MakeResponse(TUpdate updateObject)
		{
			var response = new WebhookResponse()
			{
				StatusCode = StatusCodes.Status200OK
			};

			return Task.FromResult(response);
		}
	}
}