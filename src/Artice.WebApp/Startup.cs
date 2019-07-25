using Artice.AspNetCore;
using Artice.Core.AspNetCore;
using Artice.Telegram.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
			services.AddArtice<Logic>(builder => builder
					.UseTelegramProvider(
						configuration => configuration.SetAccessToken(Configuration["Telegram:AccessToken"])
						)
				);
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
