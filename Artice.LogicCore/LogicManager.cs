using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Artice.Core;
using Artice.Core.Args;
using Artice.Core.Bots;
using Artice.Core.Models;
using Artice.LogicCore.Context;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Artice.LogicCore
{
	public class LogicManager : IIncomingMessageHandler
	{
		private readonly IContextStorage _contextStorage;

		private readonly ILogic _logic;

		//private List<SchedullerJob> _jobs;

		private readonly IOutgoingMessageProviderFactory _outgoingMessageProviderFactory;

		private readonly ILogger<LogicManager> _logger;

		public LogicManager(
			ILogger<LogicManager> logger,
			IOutgoingMessageProviderFactory outgoingMessageProviderFactory,
			ILogic logic, IContextStorage contextStorage)
		{
			_logger = logger;
			_outgoingMessageProviderFactory = outgoingMessageProviderFactory;
			_logic = logic;
			_contextStorage = contextStorage;
		}

		//todo:Добавить регистрацию запланированных задач
		//public async Task StartAsync(CancellationToken cancellationToken)
		//{
		//	_contextStorage = new ContextStorage(_serviceLocator);

		//	_logic.Initialize(_contextStorage);


		//	_jobs = new List<SchedullerJob>();

		//	if (_logic.Tasks != null)
		//	{
		//		foreach (var schedulerTask in _logic.Tasks)
		//		{
		//			var job = new SchedullerJob(schedulerTask, _contextStorage);
		//			_jobs.Add(job);
		//		}
		//	}
		//}
		

		public async Task Handle(IncomingMessage incomingMessage)
		{
			var provider = _outgoingMessageProviderFactory.GetProvider(incomingMessage.MessengerId);
			//todo: Добавить проверку на отсутсвие бота
			try
			{
				var recipient = incomingMessage.Chat != null
					? new Recipient(incomingMessage.MessengerId, incomingMessage.Chat.Id, RecipientType.Chat)
					: new Recipient(incomingMessage.MessengerId, incomingMessage.From.Id, RecipientType.User);
				var context = _contextStorage.Get(recipient);
				var response = await _logic.Answer(provider, incomingMessage, context);
				if (response != null)
				{
					provider.SendMessageAsync(response);
				}
			}
			catch (Exception ex)
			{
				_logger.LogCritical(ex, $"Bot:{provider.GetType()} Message:{ex.Message}");
				//todo: Добавить обработчик ошибок
				//_logic.OnError(provider, incomingMessage, ex);
			}
		}

	}
}