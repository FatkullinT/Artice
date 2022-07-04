using Artice.AspNetCore.AzureWebJob;
using Artice.Telegram.AspNetCore;
using Artice.Telegram.Configuration;
using Artice.Vk.AspNetCore;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Artice.AzureWebJob
{
    class Program
    {
        static async Task Main()
        {
            var builder = new HostBuilder();

            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            builder.UseEnvironment(environment);
            builder.ConfigureServices(collection
                => collection
                    .AddArtice<Logic>(
                        articeBuilder => articeBuilder
                            .UseTelegramProvider(
                                configuration => configuration
                                    .SetAccessToken(config["Telegram:AccessToken"])
                                    .UseUpdatesReceivingMethod(UpdatesReceivingMethod.LongPolling))

                    //.UseVkProvider(
                    //    configuration => configuration
                    //        .SetGroupCredentials(config["Vk:GroupId"], config["Vk:AccessToken"])
                    //        .SetWebhookVerifyToken(config["Vk:VerifyToken"])
                    //        .UseUpdatesReceivingMethod(Vk.Configuration.UpdatesReceivingMethod.LongPolling))
                    )
                );
            builder.ConfigureWebJobs(b =>
            {
                b.AddAzureStorageCoreServices();
                b.AddAzureStorageQueues();
            });
            builder.ConfigureLogging((context, b) =>
            {
                b.AddConsole();
            });
            using (var host = builder.Build())
            {
                await host.RunAsync();
            }
        }
    }
}
