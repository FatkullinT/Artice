using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Artice.Core.AspNetCore;
using Artice.Core.AspNetCore.Models;
using Artice.Core.IncomingMessages;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Artice.AspNetCore.OnPremises
{
	public class ArticeMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly IServiceProvider _rootServiceProvider;
		private readonly Dictionary<string, Type> _handlerTypes;


		public ArticeMiddleware(RequestDelegate next, IServiceProvider rootServiceProvider, string basePath)
		{
			basePath = basePath.Trim('/');
			if (!string.IsNullOrWhiteSpace(basePath))
				basePath = '/' + basePath;

			_next = next;
			_rootServiceProvider = rootServiceProvider;

			_handlerTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes())
				.Where(type => typeof(IWebhookRequestHandler).IsAssignableFrom(type))
				.SelectMany(type =>
					type.GetCustomAttributes<HandlerRouteAttribute>()
						.Select(attr => new { Type = type, attr.Route }))
				.ToDictionary(pare => $"{basePath}/{pare.Route.Trim('/')}".ToUpperInvariant(), pare => pare.Type);
		}

		public async Task InvokeAsync(HttpContext context)
		{

			if (_handlerTypes.TryGetValue(context.Request.Path.Value.ToUpperInvariant(), out var type))
			{
				using (var scope = _rootServiceProvider.CreateScope())
				{
					var updateHandler = (IWebhookRequestHandler)scope.ServiceProvider.GetService(type);
					if (updateHandler != null && await updateHandler.CheckRequest(context.Request))
					{
						var result = await updateHandler.HandleAsync(context.Request);

						await SetResponseToContext(context, result.Response);

                        if (result.IncomingMessage == null)
                            return;

                        var handlers = scope.ServiceProvider.GetServices<IIncomingMessageHandler>();

						foreach (var handler in handlers)
						{
							await handler.Handle(result.IncomingMessage);
						}

						return;
					}
				}
			}

			await _next.Invoke(context);
		}

        private async Task SetResponseToContext(HttpContext context, WebhookResponse response)
        {
			context.Response.StatusCode = response.StatusCode;

			if (string.IsNullOrEmpty(response.Body))
				return;

			await context.Response.WriteAsync(response.Body);
        }
    }
}