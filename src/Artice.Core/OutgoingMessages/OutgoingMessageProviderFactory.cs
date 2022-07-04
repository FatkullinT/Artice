using System;
using System.Collections.Generic;
using System.Linq;

namespace Artice.Core.OutgoingMessages
{
	public class OutgoingMessageProviderFactory : IOutgoingMessageProviderFactory
	{
		public OutgoingMessageProviderFactory(IEnumerable<IOutgoingMessageProvider> outgoingMessageProviders)
		{
			Providers = outgoingMessageProviders;
		}

		public IOutgoingMessageProvider GetProvider(string providerName)
		{
			return Providers
				.FirstOrDefault(provider =>
					string.Equals(providerName, provider.ChannelId, StringComparison.CurrentCultureIgnoreCase));
		}

		public IEnumerable<IOutgoingMessageProvider> Providers { get; }
	}
}