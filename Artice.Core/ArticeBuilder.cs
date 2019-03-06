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

		public List<Func<IServiceProvider, IChatBot>> ProviderFactories { get; } = new List<Func<IServiceProvider, IChatBot>>();

		public IServiceCollection Services { get; }

		public void UseProvider<TProvider>() 
			where TProvider : IChatBot
		{
			ProviderFactories.Add(serviceProvider => serviceProvider.GetService<TProvider>());
		}
	}
}