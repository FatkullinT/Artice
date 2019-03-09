using System;
using Artice.Core;
using Artice.LogicCore.Context;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Artice.LogicCore
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