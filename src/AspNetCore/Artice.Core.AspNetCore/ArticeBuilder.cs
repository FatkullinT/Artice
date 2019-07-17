using Artice.Core.OutgoingMessages;
using Microsoft.Extensions.DependencyInjection;

namespace Artice.Core.AspNetCore
{
	internal class ArticeBuilder : IArticeBuilder
	{
		public ArticeBuilder(IServiceCollection services)
		{
			Services = services;
		}

		public IServiceCollection Services { get; }

		public void UseProvider<TProvider>() 
			where TProvider : class, IOutgoingMessageProvider
		{
			Services.AddScoped<TProvider>();
			Services.AddScoped<IOutgoingMessageProvider>(provider => provider.GetRequiredService<TProvider>());
		}
	}
}