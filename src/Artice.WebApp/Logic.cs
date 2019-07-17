using System.Collections.Generic;
using System.Threading.Tasks;
using Artice.Context;
using Artice.Core.Models;
using Artice.Core.OutgoingMessages;
using Artice.Extensions;

namespace Artice.WebApp
{
	public class Logic : ILogic
	{
		public void Dispose()
		{

		}

		public IEnumerable<Task<OutgoingMessage>> Answer(IOutgoingMessageProvider outgoingMessageProvider, IncomingMessage message, IChatContext context)
		{
			if (string.Equals("/start", message.Text))
			{
				yield return Task.FromResult(message.GetResponse("Бот запущен."));
			}
			else if (string.Equals("/help", message.Text))
			{
				yield return Task.FromResult(message.GetResponse("Этот бот не умеет ничего и он очень счастлив, чего и вам желает."));
				yield return Task.FromResult(message.GetResponse("А ты так можешь?"));
			}
			else
			{
				yield return Task.FromResult(message.GetResponse($"Получено сообщение: {message.Text}"));
			}
		}
	}
}