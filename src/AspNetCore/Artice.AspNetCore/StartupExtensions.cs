using System;
using Artice.Context;
using Artice.Core.AspNetCore;
using Artice.Core.IncomingMessages;
using Microsoft.Extensions.DependencyInjection;

namespace Artice.AspNetCore
{
	public static class StartupExtensions
	{
		public static IServiceCollection AddArtice<TLogicModule>(this IServiceCollection services, Action<IArticeBuilder> buildAction = null)
			where TLogicModule : class, ILogic
		{
			return services
				.AddArtice(buildAction)
				.AddScoped<ILogic, TLogicModule>()
				.AddScoped<IIncomingMessageHandler, LogicManager>()
				.AddSingleton<IContextStorage, ContextStorage>(); 
		}
	}
}