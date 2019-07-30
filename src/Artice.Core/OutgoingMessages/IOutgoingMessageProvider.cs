using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Artice.Core.Models;
using Artice.Core.Models.Files;

namespace Artice.Core.OutgoingMessages
{
    public interface IOutgoingMessageProvider
    {
        string MessengerId { get; }

        Task SendMessageAsync(OutgoingMessage message, CancellationToken cancellationToken = default);
    }
}