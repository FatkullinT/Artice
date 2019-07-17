using Artice.Core.AspNetCore;
using Artice.Core.IncomingMessages;
using Artice.Telegram.Models;

namespace Artice.Telegram.AspNetCore
{
	[HandlerRoute("/telegram")]
	internal class TelegramRequestHandler : RequestHandler<Update>
	{
		public TelegramRequestHandler(IIncomingUpdateHandler<Update> updateHandler) : base(updateHandler)
		{}
	}
}