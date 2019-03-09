using System;
using System.Collections.Generic;
using System.Linq;
using Artice.Core.Bots;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Artice.Core
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
			return services;
		}
	}
}