using System;
using System.Threading.Tasks;
using Artice.Core.Models;

namespace Artice.Core.Queues
{
	public interface IMessageQueue : IDisposable
	{
		string Name { get; }

		Task SetMessageAsync(OutgoingMessage message);

		Task<OutgoingMessage> GetMessageAsync();
	}
}