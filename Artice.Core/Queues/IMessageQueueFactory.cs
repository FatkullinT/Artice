namespace Artice.Core.Queues
{
	public interface IMessageQueueFactory
	{
		IMessageQueue CreateQueue(string messengerId);
	}
}