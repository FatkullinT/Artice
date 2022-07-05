using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Artice.Core.AspNetCore;
using Artice.Core.AspNetCore.Models;
using Artice.Core.IncomingMessages;
using Artice.Core.Logger;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Artice.AspNetCore.AzureWebJob
{
    public class UpdatesFunctionBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<string, Type> _handlerTypes;
        private readonly ILogger _logger;

        public UpdatesFunctionBase(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _logger = serviceProvider.GetService<ILogger>();
            _handlerTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(IWebhookRequestHandler).IsAssignableFrom(type))
                .SelectMany(type =>
                    type.GetCustomAttributes<HandlerRouteAttribute>()
                        .Select(attr => new { Type = type, attr.Route }))
                .ToDictionary(pare => pare.Route.Trim('/').ToUpperInvariant(), pare => pare.Type);
        }

        protected async Task<WebhookResponse> ProcessUpdate(HttpRequest req, string provider)
        {
            if (!_handlerTypes.TryGetValue(provider.ToUpperInvariant(), out var type))
            {
                _logger.LogError($"Provider \"{provider}\" does not exist.");
                return new WebhookResponse()
                {
                    StatusCode = StatusCodes.Status404NotFound
                };
            }

            var updateHandler = (IWebhookRequestHandler)_serviceProvider.GetService(type);
            if (updateHandler == null)
            {
                _logger.LogError($"Provider \"{provider}\" is not registered.");
                return new WebhookResponse()
                {
                    StatusCode = StatusCodes.Status404NotFound
                };
            }

            if (!await updateHandler.CheckRequest(req))
            {
                _logger.LogError($"Request does not valid for provider \"{provider}\".");
                return new WebhookResponse()
                {
                    StatusCode = StatusCodes.Status404NotFound
                };
            }

            var processResult = await updateHandler.HandleAsync(req);

            if (processResult.IncomingMessage == null)
                return processResult.Response;

            var handlers = _serviceProvider.GetServices<IIncomingMessageHandler>();

            foreach (var handler in handlers)
            {
                await handler.Handle(processResult.IncomingMessage);
            }

            return processResult.Response;
        }
    }
}
