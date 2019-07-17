using System.Threading.Tasks;
using Artice.Core.Models;

namespace Artice.Core.IncomingMessages
{
	public interface IIncomingMessageHandler
	{
		Task Handle(IncomingMessage incomingMessage);
	}
}