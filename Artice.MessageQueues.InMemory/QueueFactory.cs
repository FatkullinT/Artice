using Artice.Core.Queues;

namespace Artice.MessageQueues.InMemory
{
	internal class QueueFactory : IMessageQueueFactory
	{
		public IMessageQueue CreateQueue(string messengerId)
		{
			return new MessageQueue(messengerId);
		}
	}
}