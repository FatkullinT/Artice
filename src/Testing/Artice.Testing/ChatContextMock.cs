using System;
using System.Linq.Expressions;
using Artice.Context;
using Artice.Testing.Core;
using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;

namespace Artice.Testing
{
	public class ChatContextMock : BaseMock<IChatContext, ChatContextMock>
	{
		public ChatContextMock()
		{
			var fixture = new Fixture();
			Setup(context => context.Recipient).ReturnsUsingFixture(fixture);
			Setup(context => context.CreatedAt).ReturnsUsingFixture(fixture);
			Setup(context => context.LastRequestAt).ReturnsUsingFixture(fixture);
		}

		public ChatContextMock SetRecipient(Recipient recipient)
		{
			Setup(context => context.Recipient).Returns(recipient);
			return this;
		}

		public ChatContextMock SetCreateTime(DateTime dateTime)
		{
			Setup(context => context.CreatedAt).Returns(dateTime);
			return this;
		}

		public ChatContextMock SetLastRequestTime(DateTime dateTime)
		{
			Setup(context => context.LastRequestAt).Returns(dateTime);
			return this;
		}

		public ChatContextMock AddContextValue<T>(string name, T value)
		{
			Setup(context => context.Get<T>(name)).Returns(value);
			return this;
		}

		public void VerifySet(string key, Expression<Func<object, bool>> match)
		{
			Verify(context => context.Set(key, It.Is(match)), Times.Once);
		}
	}
}