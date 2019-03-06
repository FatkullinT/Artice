using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Artice.Core
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
				.Where(type => typeof(IRequestHandler).IsAssignableFrom(type))
				.SelectMany(type =>
					type.GetCustomAttributes<HandlerRouteAttribute>()
						.Select(attr => new { Type = type, Route = attr.Route }))
				.ToDictionary(pare => $"{basePath}/{pare.Route.Trim('/')}".ToUpperInvariant(), pare => pare.Type);
		}

		public async Task InvokeAsync(HttpContext context)
		{

			if (_handlerTypes.TryGetValue(context.Request.Path.Value.ToUpperInvariant(), out var type))
			{
				using (var scope = _rootServiceProvider.CreateScope())
				{
					var handler = (IRequestHandler)scope.ServiceProvider.GetService(type);
					if (handler != null && handler.CheckRequest(context.Request))
					{
						await handler.HandleAsync(context);
						return;
					}
				}
			}

			await _next.Invoke(context);
		}
	}
}