using System.Linq;
using Artice.Context;
using Artice.Core.Models;
using Artice.Testing;
using Artice.Testing.Core;
using Artice.Tests.Mocks;
using AutoFixture;
using Xunit;

namespace Artice.Tests
{
	public class LogicManagerTests
	{
		[Fact]
		public async void Handle_DirectTextMessage_LogicModuleExecuted()
		{
			//arrange
			var fixture = new Fixture();

			var user = fixture.Create<User>();
			var messengerId = fixture.Create<string>();

			var recipient = fixture.Build<Recipient>()
				.With(r => r.RecipientType, RecipientType.User)
				.With(r => r.RecipientId, user.Id)
				.With(r => r.BotName, messengerId)
				.Create();
			
			var logicMock = new LogicMock();

			var manager = new LogicManager(
				new LoggerMock().Object,
				new OutgoingMessageProviderFactoryMock(true).Object,
				logicMock.Object,
				new ContextStorageMock(true).Object);

			var incomingMessage = fixture
				.Build<IncomingMessage>()
				.With(message => message.MessengerId, messengerId)
				.With(message => message.From, user)
				.Without(message => message.Group)
				.Without(message => message.Attachments)
				.Create();

			//act
			await manager.Handle(incomingMessage);

			//assert
			logicMock.VerifyAnswer(
				messageProvider => messageProvider.MessengerId == messengerId,
				message => message == incomingMessage,
				context => context.Recipient == recipient);
		}

		[Fact]
		public async void Handle_TextMessageFromChat_LogicModuleExecuted()
		{
			//arrange
			var fixture = new Fixture();

			var chat = fixture.Create<Group>();
			var messengerId = fixture.Create<string>();

			var recipient = fixture.Build<Recipient>()
				.With(r => r.RecipientType, RecipientType.Group)
				.With(r => r.RecipientId, chat.Id)
				.With(r => r.BotName, messengerId)
				.Create();

			var logicMock = new LogicMock();

			var manager = new LogicManager(
				new LoggerMock().Object,
				new OutgoingMessageProviderFactoryMock(true).Object,
				logicMock.Object,
				new ContextStorageMock(true).Object);

			var incomingMessage = fixture
				.Build<IncomingMessage>()
				.With(message => message.MessengerId, messengerId)
				.With(message => message.Group, chat)
				.Without(message => message.Attachments)
				.Create();

			//act
			await manager.Handle(incomingMessage);

			//assert
			logicMock.VerifyAnswer(
				messageProvider => messageProvider.MessengerId == messengerId,
				message => message == incomingMessage,
				context => context.Recipient == recipient);
		}


		[Fact]
		public async void Handle_FewMessagesToResponse_ResponseSent()
		{
			//arrange
			var fixture = new Fixture();

			var messengerId = fixture.Create<string>();

			var outgoingMessages = fixture
				.Build<OutgoingMessage>()
				.Without(message => message.Attachments)
				.CreateMany(2)
				.ToArray();

			var providerFactoryMock = new OutgoingMessageProviderFactoryMock();
			var providerMock = providerFactoryMock.AddProvider(messengerId);

			var manager = new LogicManager(
				new LoggerMock().Object,
				providerFactoryMock.Object,
				new LogicMock().Returns(outgoingMessages).Object,
				new ContextStorageMock(true).Object);

			var incomingMessage = fixture
				.Build<IncomingMessage>()
				.With(message => message.MessengerId, messengerId)
				.Without(message => message.Attachments)
				.Create();

			//act
			await manager.Handle(incomingMessage);

			//assert
			providerMock.VerifySendMessage(message => message == outgoingMessages[1]);
			providerMock.VerifySendMessage(message => message == outgoingMessages[0]);
		}
	}
}
