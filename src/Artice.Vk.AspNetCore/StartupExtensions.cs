using System;
using System.Net;
using System.Net.Http;
using Artice.Core.AspNetCore;
using Artice.Core.IncomingMessages;
using Artice.Vk.Configuration;
using Artice.Vk.HttpClients;
using Artice.Vk.Mapping;
using Artice.Vk.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Artice.Vk.AspNetCore
{
    public static class StartupExtensions
    {
        public static IArticeBuilder UseVkProvider(this IArticeBuilder builder, Action<IVkProviderConfigurator> configure, Action<IHttpClientBuilder> configureHttpClient = null)
        {
            var configurator = new VkProviderConfigurator();
            configure(configurator);

            builder.Services.AddSingleton(configurator.Configuration);
            builder.UseProvider<VkOutgoingMessageProvider>();

            switch (configurator.Configuration.UpdatesReceivingMethod)
            {
                case UpdatesReceivingMethod.Webhook:
                    builder.Services.AddScoped<VkWebhookRequestHandler>();
                    break;
                case UpdatesReceivingMethod.LongPolling:
                    builder.Services.AddSingleton<ILongPollingProcessor, LongPollingProcessor<Update>>();
                    builder.Services.AddSingleton<IInterrogator<Update>, VkInterrogator>();

                    var longPollingHttpClient = builder.Services.AddHttpClient<IVkLongPollingHttpClient, VkLongPollingHttpClient>(client =>
                    {
                        VkLongPollingHttpClient.ConfigureClient(client);
                    });
                    builder.Services.AddSingleton(provider => new Func<IVkLongPollingHttpClient>(provider.GetService<IVkLongPollingHttpClient>));
                    configureHttpClient?.Invoke(longPollingHttpClient);

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(configurator.Configuration.UpdatesReceivingMethod));
            }

            builder.Services.AddScoped<IIncomingUpdateHandler<Update>, VkUpdateHandler>();
            
            var httpClientBuilder = builder.Services.AddHttpClient<IVkHttpClient, VkHttpClient>(client =>
            {
                VkHttpClient.ConfigureClient(client);
            });

            builder.Services.AddSingleton(provider => new Func<IVkHttpClient>(provider.GetService<IVkHttpClient>));

            configureHttpClient?.Invoke(httpClientBuilder);

            builder.Services.AddScoped<IIncomingMessageMapper, IncomingMessageMapper>();
            builder.Services.AddScoped<IIncomingAttachmentMapper, IncomingAttachmentMapper>();
            builder.Services.AddScoped<IOutgoingMessageMapper, OutgoingMessageMapper>();

            return builder;
        }
    }
}