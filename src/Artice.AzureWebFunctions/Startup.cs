using Artice.AspNetCore;
using Artice.AspNetCore.AzureWebJob;
using Artice.AzureWebFunctions;
using Artice.Telegram.AspNetCore;
using Artice.Telegram.Configuration;
using Artice.Vk.AspNetCore;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

[assembly: FunctionsStartup(typeof(Startup))]

namespace Artice.AzureWebFunctions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var config = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            builder.Services.AddSingleton<IConfiguration>(config);

            builder.Services.AddArtice<Logic>(
                articeBuilder => articeBuilder
                    .UseTelegramProvider(
                        configuration => configuration
                            .SetAccessToken(config["Telegram:AccessToken"])
                            .UseUpdatesReceivingMethod(UpdatesReceivingMethod.Webhook))

                    .UseVkProvider(
                        configuration => configuration
                            .SetGroupCredentials(config["Vk:GroupId"], config["Vk:AccessToken"])
                            .SetWebhookVerifyToken(config["Vk:VerifyToken"])
                            .UseUpdatesReceivingMethod(Vk.Configuration.UpdatesReceivingMethod.Webhook))
            );
        }
    }
}