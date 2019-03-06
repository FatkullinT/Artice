using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Artice.Core
{
	public interface IRequestHandler
	{
		bool CheckRequest(HttpRequest request);

		Task HandleAsync(HttpContext context);
	}
}