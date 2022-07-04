using System;
using Artice.Core.Logger;
using Artice.Core.OutgoingMessages;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Artice.Core.AspNetCore
{
	public static class StartupExtensions
	{
        public static IServiceCollection AddArticeCore(this IServiceCollection services, Action<IArticeBuilder> build = null)
		{
			var builder = new ArticeBuilder(services);
			build?.Invoke(builder);
			services.AddScoped<IOutgoingMessageProviderFactory, OutgoingMessageProviderFactory>();
            services.AddScoped<ILogger, ArticeLogger>();
			return services;
		}
	}
}