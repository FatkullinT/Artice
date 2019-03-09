using System;
using System.Collections.Generic;

namespace Artice.LogicCore.Context
{
	public interface IContextStorage : IDisposable
	{
		IEnumerable<ChatContext> Contexts { get; }
		ChatContext Get(Recipient recipient);
		ChatContext Get(string botName, RecipientType recipientType, string recipientId);
	}
}