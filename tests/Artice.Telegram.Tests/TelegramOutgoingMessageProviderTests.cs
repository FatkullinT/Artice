using Artice.Core.Models;
using Artice.Telegram.Models.ReplyMarkups;
using Artice.Telegram.Tests.Mocks;
using AutoFixture;
using Xunit;

namespace Artice.Telegram.Tests
{
    public class TelegramOutgoingMessageProviderTests
    {
        [Fact]
        public async void SendMessageAsync_Success_MessageSent()
        {
            //arrange
            var fixture = new Fixture();

            var message = fixture.Build<OutgoingMessage>()
                .Without(outgoingMessage => outgoingMessage.Attachments)
                .Create();

            var telegramKeyboardMarkup = fixture.Build<InlineKeyboardMarkup>().Create();
            var mapper = new OutgoingMessageMapperMock().Returns(telegramKeyboardMarkup);
            var clientMock = new TelegramHttpClientMock();

            var provider = new TelegramOutgoingMessageProvider(
                mapper.Object,
                () => clientMock.Object);

            //act
            await provider.SendMessageAsync(message);

            //assert
            mapper.VerifyObjectOnMap(keyboard => ReferenceEquals(keyboard, message.InlineKeyboard));
            clientMock.VerifyPost<Telegram.Models.Message>("sendMessage",
                parameters => (string) parameters["chat_id"] == message.Group.Id
                              && (string) parameters["text"] == message.Text
                              && ReferenceEquals(parameters["reply_markup"], telegramKeyboardMarkup));
            mapper.VerifyNoOtherCalls();
            clientMock.VerifyNoOtherCalls();
        }
    }
}