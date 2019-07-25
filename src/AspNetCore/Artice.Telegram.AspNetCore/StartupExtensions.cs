using System;
using Artice.Core.AspNetCore;
using Artice.Core.IncomingMessages;
using Artice.Telegram.Mapping;
using Artice.Telegram.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Artice.Telegram.AspNetCore
{
    public static class StartupExtensions
    {
        public static IArticeBuilder UseTelegramProvider(this IArticeBuilder builder, Action<ITelegramProviderConfigurator> configure, Action<IHttpClientBuilder> configureHttpClient = null)
        {
            var configurator = new TelegramProviderConfigurator();
            configure(configurator);

            builder.Services.AddSingleton(configurator.Configuration);
            builder.UseProvider<TelegramOutgoingMessageProvider>();
            builder.Services.AddScoped<IIncomingUpdateHandler<Update>, TelegramUpdateHandler>();
            builder.Services.AddScoped<TelegramRequestHandler>();
            var httpClientBuilder = builder.Services.AddHttpClient<ITelegramHttpClient, TelegramHttpClient>(client =>
            {
                TelegramHttpClient.ConfigureClient(client);
            });

            builder.Services.AddSingleton(provider =>
                new Func<ITelegramHttpClient>(() => provider.GetService<ITelegramHttpClient>()));

            configureHttpClient?.Invoke(httpClientBuilder);

            builder.Services.AddScoped<IIncomingMessageMapper, IncomingMessageMapper>();
            builder.Services.AddScoped<IIncomingAttachmentMapper, IncomingAttachmentMapper>();
            builder.Services.AddScoped<IOutgoingMessageMapper, OutgoingMessageMapper>();

            return builder;
        }
    }
}