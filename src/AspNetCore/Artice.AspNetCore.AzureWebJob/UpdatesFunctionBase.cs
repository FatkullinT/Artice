using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Artice.Core.AspNetCore;
using Artice.Core.AspNetCore.Models;
using Artice.Core.IncomingMessages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Artice.AspNetCore.AzureWebJob
{
    public class UpdatesFunctionBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<string, Type> _handlerTypes;

        public UpdatesFunctionBase(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            _handlerTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(IWebhookRequestHandler).IsAssignableFrom(type))
                .SelectMany(type =>
                    type.GetCustomAttributes<HandlerRouteAttribute>()
                        .Select(attr => new { Type = type, attr.Route }))
                .ToDictionary(pare => pare.Route.Trim('/').ToUpperInvariant(), pare => pare.Type);
        }

        protected async Task<IActionResult> ProcessUpdate(HttpRequest req, string provider)
        {
            var actionResult = (IActionResult)new OkResult();
            if (_handlerTypes.TryGetValue(provider.ToUpperInvariant(), out var type))
            {

                var updateHandler = (IWebhookRequestHandler)_serviceProvider.GetService(type);
                if (updateHandler != null && await updateHandler.CheckRequest(req))
                {
                    var processResult = await updateHandler.HandleAsync(req);

                    actionResult = GetActionResult(processResult.Response);

                    if (processResult.IncomingMessage == null)
                        return actionResult;

                    var handlers = _serviceProvider.GetServices<IIncomingMessageHandler>();

                    foreach (var handler in handlers)
                    {
                        await handler.Handle(processResult.IncomingMessage);
                    }
                }
            }

            return actionResult;
        }

        private static IActionResult GetActionResult(WebhookResponse response)
        {
            return new ObjectResult(response.Body) 
            { 
                StatusCode = response.StatusCode
            };
        }
    }
}
