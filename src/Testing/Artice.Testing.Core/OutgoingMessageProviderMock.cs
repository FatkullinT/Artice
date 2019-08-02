using System;
using System.Linq.Expressions;
using System.Threading;
using Artice.Core.Models;
using Artice.Core.OutgoingMessages;
using Moq;

namespace Artice.Testing.Core
{
	public class OutgoingMessageProviderMock : BaseMock<IOutgoingMessageProvider, OutgoingMessageProviderMock>
	{
		public OutgoingMessageProviderMock(string providerId)
		{
			Setup(provider => provider.ChannelId).Returns(providerId);
		}

		public void VerifySendMessage(Expression<Func<OutgoingMessage, bool>> match, Times times)
		{
			Verify(provider => provider.SendMessageAsync(It.Is(match), It.IsAny<CancellationToken>()), times);
		}

		public void VerifySendMessage(Expression<Func<OutgoingMessage, bool>> match)
		{
			VerifySendMessage(match, Times.Once());
		}
	}
}