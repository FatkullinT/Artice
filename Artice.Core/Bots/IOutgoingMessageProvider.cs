using System;
using System.Threading;
using System.Threading.Tasks;
using Artice.Core.Args;
using Artice.Core.Models;

namespace Artice.Core.Bots
{
	public interface IOutgoingMessageProvider
	{
		string MessengerId { get; }

		Task SendMessageAsync(OutgoingMessage message, CancellationToken cancellationToken = new CancellationToken());

		Task GetFileContentAsync(FileReference file, CancellationToken cancellationToken = new CancellationToken());
	}
}