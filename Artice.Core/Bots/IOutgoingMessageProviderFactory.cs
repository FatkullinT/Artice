using System;
using System.Collections.Generic;

namespace Artice.Core.Bots
{
	public interface IOutgoingMessageProviderFactory
	{
		IOutgoingMessageProvider GetProvider(string botName);

		IEnumerable<IOutgoingMessageProvider> Providers { get; }
	}
}