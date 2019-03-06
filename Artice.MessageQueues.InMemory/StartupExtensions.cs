using Artice.Core.Bots;
using Artice.Core.Queues;
using Microsoft.Extensions.DependencyInjection;

namespace Artice.MessageQueues.InMemory
{
	public static class StartupExtensions
	{
		public static IServiceCollection AddInMemoryMessageQueue(this IServiceCollection services)
		{
			services.AddScoped<IMessageQueueFactory, QueueFactory>();
			return services;
		}
	}
}