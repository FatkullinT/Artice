using System;
using System.Threading;
using System.Threading.Tasks;
using Artice.Core.Args;
using Artice.Core.Models;
using Artice.Core.Queues;
using Microsoft.Extensions.Logging;

namespace Artice.Core.Bots
{
    public abstract class ChatBotBase : IChatBot
    {
        public abstract event EventHandler<UpdateEventArgs> Update;

        public abstract string Name { get; }

        protected abstract int SendingLimit { get; }

        private readonly IMessageQueue _sendingMessageQueue;

        private readonly Sender _sender;

        protected ChatBotBase(IMessageQueueFactory factory, ILogger logger)
        {
            _sendingMessageQueue = factory.CreateQueue(Name);
            _sender = new Sender(_sendingMessageQueue, SendMessageAsync, SendingLimit, logger);
        }

        protected abstract Task<bool> SendMessageAsync(OutgoingMessage message,
            CancellationToken cancellationToken = new CancellationToken());

	    public async Task MessageToSendQueueAsync(OutgoingMessage message)
	    {
			await _sendingMessageQueue.SetMessageAsync(message);
		}

	    public abstract Task GetFileContentAsync(FileReference file,
            CancellationToken cancellationToken = new CancellationToken());

        public void Dispose()
        {
            _sender.Dispose();
            _sendingMessageQueue.Dispose();
        }
    }
}