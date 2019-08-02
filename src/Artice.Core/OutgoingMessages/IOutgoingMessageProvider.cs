using System.Threading;
using System.Threading.Tasks;
using Artice.Core.Models;

namespace Artice.Core.OutgoingMessages
{
    public interface IOutgoingMessageProvider
    {
        string ChannelId { get; }

        Task SendMessageAsync(OutgoingMessage message, CancellationToken cancellationToken = default);
    }
}