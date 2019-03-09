using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Artice.Core;
using Artice.Core.Bots;
using Artice.Core.Models;
using Artice.Telegram.Models;
using Artice.Telegram.Models.Enums;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;


namespace Artice.Telegram
{
	[HandlerRoute("/telegram")]
	internal class TelegramUpdateHandler : UpdateHandler
	{
		private readonly IMapper _mapper;

		public TelegramUpdateHandler(IMapper mapper)
		{
			_mapper = mapper;
		}

		protected override Task<IncomingMessage> ConvertJsonContent(string content)
		{
			var updateModel = JsonConvert.DeserializeObject<Update>(content);
			var incomingMessage = GetMessage(updateModel);
			//todo: Добавить проверку на успешную десериализацию
			incomingMessage.MessengerId = TelegramOutgoingMessageProvider.TelegramId;
			if (updateModel.Type == UpdateType.CallbackQueryUpdate)
			{
				//todo: Добавить отправку ответа о получении Callback
			}

			return Task.FromResult(incomingMessage);
		}

		private IncomingMessage GetMessage(Update update)
		{
			switch (update.Type)
			{
				case UpdateType.MessageUpdate:
				{
					return _mapper.Map<IncomingMessage>(update.Message);
				}
				case UpdateType.CallbackQueryUpdate:
				{
					return _mapper.Map<IncomingMessage>(update.CallbackQuery);
				}
				default:
				{
					return null;
				}
			}
		}

	}
}