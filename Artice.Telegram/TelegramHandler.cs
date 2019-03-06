using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Artice.Core;
using Artice.Core.Bots;
using Artice.Telegram.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;


namespace Artice.Telegram
{
	[HandlerRoute("/telegram")]
	internal class TelegramHandler : IRequestHandler
	{
		private readonly TelegramBot _telegramBot;

		public TelegramHandler(IBotStorage storage)
		{
			_telegramBot = storage.GetBot<TelegramBot>();
		}

		public bool CheckRequest(HttpRequest request)
		{
			return request.Method == HttpMethod.Post.Method;
		}

		public async Task HandleAsync(HttpContext context)
		{
			using (Stream receiveStream = context.Request.Body)
			{
				using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8))
				{
					var content = await readStream.ReadToEndAsync();
					var updateModel = JsonConvert.DeserializeObject<Update>(content);
					_telegramBot.OnUpdatesReceived(new[] { updateModel });
				}
			}
		}
	}
}