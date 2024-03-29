﻿using System;
using System.Threading.Tasks;
using Artice.Context;
using Artice.Core.Exceptions;
using Artice.Core.IncomingMessages;
using Artice.Core.Logger;
using Artice.Core.Models;
using Artice.Core.OutgoingMessages;

namespace Artice
{
    public class LogicManager : IIncomingMessageHandler
    {
        private readonly IContextStorage _contextStorage;

        private readonly ILogic _logic;

        //private List<SchedullerJob> _jobs;

        private readonly IOutgoingMessageProviderFactory _outgoingMessageProviderFactory;

        private readonly ILogger _logger;

        public LogicManager(
            ILogger logger,
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
                var recipient = incomingMessage.Group != null
                    ? new Recipient(incomingMessage.MessengerId, incomingMessage.Group.Id, RecipientType.Group)
                    : new Recipient(incomingMessage.MessengerId, incomingMessage.From.Id, RecipientType.User);
                var context = _contextStorage.Get(recipient);
                var responses = _logic.Answer(provider, incomingMessage, context);
                if (responses != null)
                {
                    foreach (var responseTask in responses)
                    {
                        var message = await responseTask;
                        await provider.SendMessageAsync(message);
                    }
                }
            }
            catch (ArticeExecutionException ex)
            {
                _logger.LogError(ex, $"Channel:{ex.BotApiIdentifier} Message:{ex.Message}");
                //throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"OutgoingMessageProvider:{provider.GetType()} Message:{ex.Message}");
                //throw;
            }

        }

    }
}