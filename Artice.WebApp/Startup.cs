using System.Net.Http;
using Artice.Core;
using Artice.LogicCore;
using Artice.MessageQueues.InMemory;
using Artice.Telegram;
using Extreme.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Swashbuckle.AspNetCore.Swagger;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

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
			services.AddArtice<Logic>(builder =>
				builder.UseTelegramProvider(configuration => configuration
						.SetAccessToken(Configuration["Telegram:AccessToken"])
						.UseHttpMessageHandler(new ProxyHandler(new Socks5ProxyClient("127.0.0.1", 9150)))));

			services.AddInMemoryMessageQueue();
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			app.UseArtice("/Artice");

			app.Run(async context =>
			{
				await context.Response.WriteAsync("Hello from non-Map delegate.");
			});
		}
	}
}
