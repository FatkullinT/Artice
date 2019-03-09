using Artice.Core.Bots;
using Microsoft.Extensions.DependencyInjection;

namespace Artice.Core
{
	public interface IArticeBuilder
	{
		IServiceCollection Services { get; }

		void UseProvider<TProvider>()
			where TProvider : class, IOutgoingMessageProvider;
	}
}