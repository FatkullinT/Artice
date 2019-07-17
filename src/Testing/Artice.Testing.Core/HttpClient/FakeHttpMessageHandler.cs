using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Artice.Testing.Core.HttpClient
{
	public abstract class FakeHttpMessageHandler : DelegatingHandler
	{
		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			return await InnerSendAsync(request, cancellationToken);
		}

		public abstract Task<HttpResponseMessage> InnerSendAsync(HttpRequestMessage request,
			CancellationToken cancellationToken);
	}
}