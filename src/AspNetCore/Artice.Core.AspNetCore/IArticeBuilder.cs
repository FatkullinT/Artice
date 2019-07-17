using Artice.Core.OutgoingMessages;
using Microsoft.Extensions.DependencyInjection;

namespace Artice.Core.AspNetCore
{
	public interface IArticeBuilder
	{
		IServiceCollection Services { get; }

		void UseProvider<TProvider>()
			where TProvider : class, IOutgoingMessageProvider;
	}
}