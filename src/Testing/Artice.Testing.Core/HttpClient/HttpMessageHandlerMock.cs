using System;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Threading;
using Moq;

namespace Artice.Testing.Core.HttpClient
{
	public class HttpMessageHandlerMock : BaseMock<FakeHttpMessageHandler, HttpMessageHandlerMock>
	{

		public HttpMessageHandlerMock(HttpResponseMessage responseMessage)
		{
			Setup(handler => handler.InnerSendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(responseMessage);
		}

		public HttpMessageHandlerMock() 
			: this(new HttpResponseMessage(HttpStatusCode.OK))
		{}

		public void Verify(Expression<Func<HttpRequestMessage, bool>> match)
		{
			Verify(handler => handler.InnerSendAsync(It.Is(match), It.IsAny<CancellationToken>()), Times.Once);
		}
	}
}