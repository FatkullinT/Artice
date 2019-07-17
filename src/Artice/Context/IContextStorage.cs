using System;
using System.Collections.Generic;

namespace Artice.Context
{
	public interface IContextStorage : IDisposable
	{
		IEnumerable<ChatContext> Contexts { get; }
		IChatContext Get(Recipient recipient);
		IChatContext Get(string botName, RecipientType recipientType, string recipientId);
	}
}