using Artice.AspNetCore;
using Artice.Core.AspNetCore;
using Artice.Telegram;
using Artice.Telegram.AspNetCore;
using Artice.Telegram.Configuration;
using Artice.Vk.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Artice.WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddArtice<Logic>(builder => builder
                
                .UseTelegramProvider(
                    configuration => configuration
                        .SetAccessToken(Configuration["Telegram:AccessToken"])
                        .UseUpdatesReceivingMethod(UpdatesReceivingMethod.LongPolling))

                .UseVkProvider(
                    configuration => configuration
                    .SetGroupCredentials(Configuration["Vk:GroupId"], Configuration["Vk:AccessToken"])
                    .SetWebhookVerifyToken(Configuration["Vk:VerifyToken"])
                    .UseUpdatesReceivingMethod(Vk.Configuration.UpdatesReceivingMethod.LongPolling))
            );
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseArtice("/Artice");

            app.Run(async context => { await context.Response.WriteAsync("Hello from non-Map delegate."); });
        }
    }
}
