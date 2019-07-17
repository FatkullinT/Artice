using System.Threading.Tasks;
using Artice.Core.Models;
using Microsoft.AspNetCore.Http;

namespace Artice.Core.AspNetCore
{
	public interface IRequestHandler
	{
		Task<bool> CheckRequest(HttpRequest request);

		Task<IncomingMessage> HandleAsync(HttpContext context);
	}
}