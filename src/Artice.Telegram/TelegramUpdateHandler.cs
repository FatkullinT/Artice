using Artice.Core.IncomingMessages;
using Artice.Core.Models;
using Artice.Telegram.Mapping;
using Artice.Telegram.Models;
using Artice.Telegram.Models.Enums;

namespace Artice.Telegram
{
	public class TelegramUpdateHandler : IIncomingUpdateHandler<Update>
	{
		private readonly IIncomingMessageMapper _mapper;

		public TelegramUpdateHandler(IIncomingMessageMapper mapper)
		{
			_mapper = mapper;
		}

		public IncomingMessage Handle(Update update)
		{
			switch (update.Type)
			{
				case UpdateType.MessageUpdate:
					{
						return _mapper.Map(update.Message);
					}
				case UpdateType.CallbackQueryUpdate:
					{
						//todo: Add response sending about callback receiving
						return _mapper.Map(update.CallbackQuery);
					}
				default:
					{
						//todo: Check success deserialization
						return null;
					}
			}
		}
	}
}