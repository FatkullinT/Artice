using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Artice.Core.Args;
using Artice.Core.Bots;
using Artice.LogicCore.Context;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Artice.LogicCore
{
    public class BotLogicManager : IHostedService
    {
        private ContextStorage _contextStorage;

        private IBotLogic _logic;

        private List<SchedullerJob> _jobs;

        private readonly IServiceLocator _serviceLocator;

        private IBotStorage _botStorage;

        private ILogger<BotLogicManager> _logger;

        public BotLogicManager(IServiceLocator serviceLocator, ILogger<BotLogicManager> logger, IBotStorage botStorage, IBotLogic botLogic)
        {
	        _serviceLocator = serviceLocator;
	        _logger = logger;
	        _botStorage = botStorage;
	        _logic = botLogic;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
			_contextStorage = new ContextStorage(_serviceLocator);

			_logic.Initialize(_contextStorage);

			foreach (var bot in _botStorage.Bots)
			{
				bot.Update += OnChatUpdate;
			}
			
			_jobs = new List<SchedullerJob>();

			if (_logic.Tasks != null)
			{
				foreach (var schedulerTask in _logic.Tasks)
				{
					var job = new SchedullerJob(schedulerTask, _contextStorage);
					_jobs.Add(job);
				}
			}
		}

        public Task StopAsync(CancellationToken cancellationToken)
        {
			_contextStorage.Dispose();
			foreach (var bot in _botStorage.Bots)
			{
				bot.Update -= OnChatUpdate;
			}
			return Task.CompletedTask;
		}

        private async void OnChatUpdate(object sender, UpdateEventArgs updateEventArgs)
        {
	        _serviceLocator.CreateScope();
	        foreach (var message in updateEventArgs.Messages)
	        {
		        try
		        {
			        var recipient = message.Chat != null
				        ? new Recipient(updateEventArgs.ChatBot.Name, message.Chat.Id, RecipientType.Chat)
				        : new Recipient(updateEventArgs.ChatBot.Name, message.From.Id, RecipientType.User);
			        var context = _contextStorage.Get(recipient);
			        var response = await _logic.Answer(updateEventArgs.ChatBot, message, context);
			        if (response != null)
			        {
				        updateEventArgs.ChatBot.MessageToSendQueueAsync(response);
			        }
		        }
		        catch (Exception ex)
		        {
			        _logger.LogCritical(ex, $"Bot:{updateEventArgs.ChatBot.GetType()} Message:{ex.Message}");
			        _logic.OnError(updateEventArgs.ChatBot, message, ex);
		        }
	        }
	        _serviceLocator.DeleteScope();
        }
	}
}