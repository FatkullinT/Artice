using System.Threading;
using System.Threading.Tasks;
using Artice.Core.Models;

namespace Artice.Core.IncomingMessages
{
	public interface IIncomingUpdateHandler<in TUpdate>
	{
		Task<IncomingMessage> HandleAsync(TUpdate incomingUpdate, CancellationToken cancellationToken = default);
	}
}