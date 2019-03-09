using System.Threading.Tasks;
using Artice.Core.Models;

namespace Artice.Core
{
	public interface IIncomingMessageHandler
	{
		Task Handle(IncomingMessage incomingMessage);
	}
}