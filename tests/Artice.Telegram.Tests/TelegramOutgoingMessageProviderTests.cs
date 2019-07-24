using Artice.Core.Models;
using Artice.Telegram.MapConfig;
using Artice.Telegram.Tests.Mocks;
using AutoFixture;
using AutoMapper;
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
            var mapper = new Mapper(new MapperConfiguration(expression => expression.AddProfile(new TelegramMapProfile())));
            var clientMock = new TelegramHttpClientMock();
            var config = fixture.Create<TelegramProviderConfiguration>();

            var provider = new TelegramOutgoingMessageProvider(
                mapper,
                () => clientMock.Object);

            var message = fixture.Build<OutgoingMessage>()
                .Without(outgoingMessage => outgoingMessage.Attachments)
                .Create();

            //act
            await provider.SendMessageAsync(message);

            //assert
            clientMock.VerifyPost<Telegram.Models.Message>("sendMessage",
            parameters => (string)parameters["chat_id"] == message.Chat.Id
                          && (string)parameters["text"] == message.Text);
        }
    }
}