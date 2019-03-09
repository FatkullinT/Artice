using System;
using System.Collections.Generic;
using Artice.Core.Bots;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Artice.Core
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