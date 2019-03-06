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
	public class Logic : IBotLogic
	{
		public void Dispose()
		{
			
		}

		public IEnumerable<SchedullerTask> Tasks { get; }

		public void Initialize(ContextStorage contextStorage)
		{
		}

		public Task<OutgoingMessage> Answer(IChatBot chatBot, IncomingMessage message, ChatContext context)
		{
			return Task.FromResult(message.GetResponse($"Получено сообщение: {message.Text}"));
		}

		public void OnError(IChatBot chatBot, IncomingMessage message, Exception exception)
		{
		}
	}
}