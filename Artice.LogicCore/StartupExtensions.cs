using System;
using Artice.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Artice.LogicCore
{
	public static class StartupExtensions
	{
		public static IServiceCollection AddArtice<TLogicModule>(this IServiceCollection services, Action<IArticeBuilder> buildAction = null)
			where TLogicModule : class, IBotLogic
		{
			return services
				.AddArtice(buildAction)
				.AddSingleton<IBotLogic, TLogicModule>()
				.AddSingleton<IServiceLocator, ServiceLocator>()
				.AddSingleton<IHostedService, BotLogicManager>(); 
		}
	}
}