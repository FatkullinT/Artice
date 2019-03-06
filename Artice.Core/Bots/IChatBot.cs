using System;
using System.Threading;
using System.Threading.Tasks;
using Artice.Core.Args;
using Artice.Core.Models;

namespace Artice.Core.Bots
{
    public interface  IChatBot : IDisposable
    {
        event EventHandler<UpdateEventArgs> Update;

        string Name { get; }

        Task MessageToSendQueueAsync(OutgoingMessage message);

        Task GetFileContentAsync(FileReference file, CancellationToken cancellationToken = new CancellationToken());
    }
}