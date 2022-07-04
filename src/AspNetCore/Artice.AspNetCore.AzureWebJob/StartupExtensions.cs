using System;
using Artice.Core.AspNetCore;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.DependencyInjection;

namespace Artice.AspNetCore.AzureWebJob
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddArtice<TLogicModule>(
            this IServiceCollection services,
            Action<IArticeBuilder> build = null)
            where TLogicModule : class, ILogic
        {
            services.AddArticeAzureServices();
            services.AddArticeWithLogic<TLogicModule>(build);
            return services;
        }

        public static IServiceCollection AddArticeAzureServices(this IServiceCollection services)
        {
            services.AddSingleton<IJobHost, LongPollingJobHost>();
            return services;
        }
    }
}