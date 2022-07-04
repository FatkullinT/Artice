using System;
using Artice.Core.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Artice.AspNetCore.OnPremises
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

        public static IServiceCollection AddArtice<TLogicModule>(
            this IServiceCollection services,
            Action<IArticeBuilder> build = null)
            where TLogicModule : class, ILogic
        {
            services.AddArticeOnPremisesServices();
            services.AddArticeWithLogic<TLogicModule>(build);
            return services;
        }

        public static IServiceCollection AddArticeOnPremisesServices(this IServiceCollection services)
		{
            services.AddSingleton<ArticeMiddleware>();
            services.AddSingleton<IHostedService, LongPollingService>();
            return services;
		}
	}
}