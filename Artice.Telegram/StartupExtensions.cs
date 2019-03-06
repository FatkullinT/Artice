using System;
using System.Linq;
using System.Net.Http;
using Artice.Core;
using Artice.Core.Bots;
using Artice.Core.Queues;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Artice.Telegram
{
	public static class StartupExtensions
	{
		public static IArticeBuilder UseTelegramProvider(this IArticeBuilder builder, Action<ITelegramProviderConfiguration> configure)
		{
			var configuration = new TelegramProviderConfiguration();
			configure(configuration);

			builder.Services.AddScoped<TelegramHandler>();

			builder.Services.AddScoped(provider => new TelegramBot(
				provider.GetService<IMessageQueueFactory>(),
				provider.GetService<ILogger<TelegramBot>>(),
				provider.GetService<IMapper>(),
				configuration));

			builder.UseProvider<TelegramBot>();

			if (builder.Services.All(descriptor => descriptor.ServiceType != typeof(IMapper)))
				builder.Services.AddAutoMapper();

			return builder;
		}
	}
}