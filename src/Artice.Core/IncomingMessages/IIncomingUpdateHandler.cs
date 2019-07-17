using Artice.Core.Models;

namespace Artice.Core.IncomingMessages
{
	public interface IIncomingUpdateHandler<in TUpdate>
	{
		IncomingMessage Handle(TUpdate incomingUpdate);
	}
}