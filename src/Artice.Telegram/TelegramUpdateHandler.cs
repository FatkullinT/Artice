using Artice.Core.IncomingMessages;
using Artice.Core.Models;
using Artice.Telegram.Models;
using Artice.Telegram.Models.Enums;
using AutoMapper;

namespace Artice.Telegram
{
	public class TelegramUpdateHandler : IIncomingUpdateHandler<Update>
	{
		private readonly IMapper _mapper;

		public TelegramUpdateHandler(IMapper mapper)
		{
			_mapper = mapper;
		}

		public IncomingMessage Handle(Update update)
		{
			switch (update.Type)
			{
				case UpdateType.MessageUpdate:
					{
						return _mapper.Map(update.Message, new IncomingMessage { MessengerId = Consts.TelegramId });
					}
				case UpdateType.CallbackQueryUpdate:
					{
						//todo: Add response sending about callback receiving
						return _mapper.Map(update.CallbackQuery, new IncomingMessage { MessengerId = Consts.TelegramId });
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