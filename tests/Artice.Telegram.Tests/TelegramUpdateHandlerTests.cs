
using Artice.Core.Models;
using Artice.Telegram.Mapping;
using Artice.Telegram.Models;
using Artice.Telegram.Models.Enums;
using Artice.Telegram.Tests.Mocks;
using Artice.Testing.Core;
using AutoFixture;
using Xunit;
using Message = Artice.Telegram.Models.Message;

namespace Artice.Telegram.Tests
{
    public class TelegramUpdateHandlerTests
    {
        [Fact]
        public void Convert_TextMessage_IncomingMessageWithText()
        {
            //arrange
            var fixture = new Fixture();
            var update = new Update()
            {
                Message = fixture.Build<Message>()
                    .Without(m => m.ReplyToMessage)
                    .Without(m => m.PinnedMessage)
                    .Create()
            };
            var expectedMessage = fixture.Build<IncomingMessage>().Without(m => m.Attachments).Create();
            var mapper = new IncomingMessageMapperMock().Returns(expectedMessage);
            var handler = new TelegramUpdateHandler(mapper.Object);

            //act
            var message = handler.Handle(update);

            //assert
            Assert.Same(expectedMessage, message);
            mapper.VerifyMessageOnMap(m => ReferenceEquals(update.Message, m));
            mapper.VerifyNoOtherCalls();
        }

        [Fact]
        public void Convert_Callback_IncomingMessageWithCommand()
        {
            //arrange
            var fixture = new Fixture();
            var expectedMessage = fixture.Build<IncomingMessage>().Without(m => m.Attachments).Create();
            var mapper = new IncomingMessageMapperMock().Returns(expectedMessage);
            var handler = new TelegramUpdateHandler(mapper.Object);
            var update = new Update()
            {
                CallbackQuery = fixture.Build<CallbackQuery>()
                    .Without(query => query.Message)
                    .Create()
            };

            //act
            var message = handler.Handle(update);

            //assert
            Assert.Same(expectedMessage, message);
            mapper.VerifyCallbackQueryOnMap(cq => ReferenceEquals(update.CallbackQuery, cq));

        }
    }
}