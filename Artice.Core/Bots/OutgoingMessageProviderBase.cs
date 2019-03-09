using System;
using System.Threading;
using System.Threading.Tasks;
using Artice.Core.Args;
using Artice.Core.Models;
using Artice.Core.Queues;
using Microsoft.Extensions.Logging;

namespace Artice.Core.Bots
{
    public abstract class OutgoingMessageProviderBase : IOutgoingMessageProvider
    {
        public abstract string MessengerId { get; }

        protected abstract int SendLimitPerSecond { get; }

        public abstract Task SendMessageAsync(OutgoingMessage message,
            CancellationToken cancellationToken = new CancellationToken());
	    

	    public abstract Task GetFileContentAsync(FileReference file,
            CancellationToken cancellationToken);

    }
}