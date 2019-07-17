using System;
using Artice.Core.Logger;
using Artice.Core.OutgoingMessages;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Artice.Core.AspNetCore
{
	public static class StartupExtensions
	{
		public static IApplicationBuilder UseArtice(this IApplicationBuilder builder, string baseUrl = "/updates")
		{
			if (baseUrl == null)
				throw new ArgumentNullException(nameof(baseUrl));

			builder.UseMiddleware<ArticeMiddleware>((object)baseUrl);
			return builder;
		}

		

		public static IServiceCollection AddArtice(this IServiceCollection services, Action<IArticeBuilder> build = null)
		{
			var builder = new ArticeBuilder(services);
			build?.Invoke(builder);
			services.AddScoped<IOutgoingMessageProviderFactory, OutgoingMessageProviderFactory>();
			services.AddSingleton<ArticeMiddleware>();
			services.AddScoped<ILogger, ArticeLogger>();
			return services;
		}
	}
}