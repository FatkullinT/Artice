using System.Collections.Generic;

namespace Artice.Core.OutgoingMessages
{
	public interface IOutgoingMessageProviderFactory
	{
		IOutgoingMessageProvider GetProvider(string botName);

		IEnumerable<IOutgoingMessageProvider> Providers { get; }
	}
}