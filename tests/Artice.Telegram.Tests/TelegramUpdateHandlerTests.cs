
using Artice.Core.Models;
using Artice.Telegram.Models;
using Artice.Telegram.Tests.Mocks;
using AutoFixture;
using Xunit;
using Message = Artice.Telegram.Models.Message;

namespace Artice.Telegram.Tests
{
    public class TelegramUpdateHandlerTests
    {
        [Fact]
        public async void HandleAsync_TextMessage_IncomingMessageWithText()
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
            var client = new TelegramHttpClientMock();
            var handler = new TelegramUpdateHandler(mapper.Object, ()=>client.Object);

            //act
            var message = await handler.HandleAsync(update);

            //assert
            client.VerifyNoOtherCalls();
            Assert.Same(expectedMessage, message);
            mapper.VerifyMessageOnMap(m => ReferenceEquals(update.Message, m));
            mapper.VerifyNoOtherCalls();
        }

        [Fact]
        public async void HandleAsync_Callback_IncomingMessageWithCommand()
        {
            //arrange
            var fixture = new Fixture();
            var expectedMessage = fixture.Build<IncomingMessage>().Without(m => m.Attachments).Create();
            var mapper = new IncomingMessageMapperMock().Returns(expectedMessage);
            var client = new TelegramHttpClientMock();
            var handler = new TelegramUpdateHandler(mapper.Object, () => client.Object);
            var update = new Update()
            {
                CallbackQuery = fixture.Build<CallbackQuery>()
                    .Without(query => query.Message)
                    .Create()
            };

            //act
            var message = await handler.HandleAsync(update);

            //assert
            client.VerifyPost<bool>("answerCallbackQuery", parameters => string.Equals((string)parameters["callback_query_id"], update.CallbackQuery.Id));
            Assert.Same(expectedMessage, message);
            mapper.VerifyCallbackQueryOnMap(cq => ReferenceEquals(update.CallbackQuery, cq));

        }
    }
}