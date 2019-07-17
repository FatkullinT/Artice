using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Artice.Context;
using Artice.Core.Models;
using Artice.Core.OutgoingMessages;

namespace Artice
{
    public interface ILogic : IDisposable
    {
		IEnumerable<Task<OutgoingMessage>> Answer(
			IOutgoingMessageProvider outgoingMessageProvider,
			IncomingMessage message,
			IChatContext context);
	}
}