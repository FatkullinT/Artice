using Artice.Core.AspNetCore;
using Artice.Core.IncomingMessages;
using Artice.Telegram.Models;

namespace Artice.Telegram.AspNetCore
{
	[HandlerRoute("/telegram")]
	internal class TelegramWebhookRequestHandler : WebhookRequestHandler<Update>
	{
		public TelegramWebhookRequestHandler(IIncomingUpdateHandler<Update> updateHandler) : base(updateHandler)
		{}
	}
}