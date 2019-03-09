using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Artice.Core.Bots;
using Artice.Core.Models;
using Artice.LogicCore;
using Artice.LogicCore.Context;
using Artice.LogicCore.Extensions;

namespace Artice.WebApp
{
	public class Logic : ILogic
	{
		public void Dispose()
		{

		}

		public Task<OutgoingMessage> Answer(IOutgoingMessageProvider outgoingMessageProvider, IncomingMessage message, ChatContext context)
		{
			if (string.Equals("/start", message.Text))
				return Task.FromResult(message.GetResponse("Бот запущен."));

			if (string.Equals("/help", message.Text))
				return Task.FromResult(message.GetResponse("Этот бот не умеет ничего и он очень счастлив, чего и вам желает."));

			return Task.FromResult(message.GetResponse($"Получено сообщение: {message.Text}"));
		}
	}
}