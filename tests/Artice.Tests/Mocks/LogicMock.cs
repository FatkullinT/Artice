using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Artice.Context;
using Artice.Core.Models;
using Artice.Core.OutgoingMessages;
using Artice.Testing.Core;
using Moq;

namespace Artice.Tests.Mocks
{
	public class LogicMock : BaseMock<ILogic, LogicMock>
	{
		public LogicMock Returns(IEnumerable<OutgoingMessage> messages)
		{
			Setup(logic => logic.Answer(It.IsAny<IOutgoingMessageProvider>(), It.IsAny<IncomingMessage>(),
				It.IsAny<IChatContext>())).Returns(messages.Select(Task.FromResult));

			return this;
		}

		public LogicMock Returns(params OutgoingMessage[] messages)
		{
			return Returns((IEnumerable<OutgoingMessage>)messages);
		}

		public void VerifyAnswer(
			Expression<Func<IOutgoingMessageProvider, bool>> providerMatch,
			Expression<Func<IncomingMessage, bool>> messageMatch,
			Expression<Func<IChatContext, bool>> contextMatch)
		{
			Verify(logic => logic.Answer(It.Is(providerMatch), It.Is(messageMatch), It.Is(contextMatch)), Times.Once);
		}
	}
}