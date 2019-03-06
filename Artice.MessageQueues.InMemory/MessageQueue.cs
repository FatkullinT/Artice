using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Artice.Core.Models;
using Artice.Core.Queues;

namespace Artice.MessageQueues.InMemory
{
	internal class MessageQueue : IMessageQueue
	{
		private readonly Queue<OutgoingMessage> _innerQueue = new Queue<OutgoingMessage>();

		public MessageQueue(string name)
		{
			Name = name;
		}

		public void Dispose()
		{
			
		}

		public string Name { get; }

		public Task SetMessageAsync(OutgoingMessage message)
		{
			_innerQueue.Enqueue(message);
			return Task.CompletedTask;
		}

		public Task<OutgoingMessage> GetMessageAsync()
		{
			return Task.FromResult(_innerQueue.Any() ? _innerQueue.Dequeue() : null);
		}
	}
}