using System.Globalization;
using System.Linq;
using Artice.Core.Models;
using Artice.Telegram.Mapping;
using Artice.Telegram.Models;
using Artice.Telegram.Models.Enums;
using Artice.Telegram.Tests.Mocks;
using AutoFixture;
using Xunit;
using Chat = Artice.Telegram.Models.Chat;
using User = Artice.Telegram.Models.User;

namespace Artice.Telegram.Tests
{
    public class IncomingMessageMapperTests
    {
        [Fact]
        public void Map_TextMessage_IncomingMessage()
        {
            //arrange
            var fixture = new Fixture();
            var message = new Telegram.Models.Message()
            {
                Id = fixture.Create<int>(),
                Caption = fixture.Create<string>(),
                Chat = fixture.Create<Chat>(),
                From = fixture.Create<User>(),
                Text = fixture.Create<string>()
            };
            var attachmentMapper = new IncomingAttachmentMapperMock().Returns(Enumerable.Empty<Attachment>());
            var mapper = new IncomingMessageMapper(attachmentMapper.Object);

            //act
            var incomingMessage = mapper.Map(message);

            //assert
            attachmentMapper.VerifyAttachmentOnMap(m => ReferenceEquals(m, message));
            Assert.Empty(incomingMessage.Attachments);
            Assert.Equal(message.Id.ToString(CultureInfo.InvariantCulture), incomingMessage.Id);
            Assert.Equal(message.Time, incomingMessage.Time);
            Assert.Equal(message.From.Id.ToString(CultureInfo.InvariantCulture), incomingMessage.From.Id);
            Assert.Equal(message.Text, incomingMessage.Text);
            Assert.Equal(Consts.ChannelId, incomingMessage.MessengerId);
            Assert.Null(incomingMessage.CallbackData);
        }

        [Fact]
        public void Map_TextMessageWithAttachment_IncomingMessage()
        {
            //arrange
            var fixture = new Fixture();
            var message = new Telegram.Models.Message()
            {
                Id = fixture.Create<int>(),
                Photo = fixture.CreateMany<PhotoSize>(3).ToArray(),
                Caption = fixture.Create<string>(),
                Chat = fixture.Create<Chat>(),
                From = fixture.Create<User>(),
                Text = fixture.Create<string>()
            };
            var image = fixture.Build<Image>().Without(i => i.File).Create();
            var attachmentMapper = new IncomingAttachmentMapperMock().Returns(Enumerable.Repeat((Attachment)image, 1));
            var mapper = new IncomingMessageMapper(attachmentMapper.Object);

            //act
            var incomingMessage = mapper.Map(message);

            //assert
            attachmentMapper.VerifyAttachmentOnMap(m => ReferenceEquals(m, message));
            var attachment = Assert.Single(incomingMessage.Attachments);
            Assert.Same(image, attachment);
            Assert.Equal(message.Id.ToString(CultureInfo.InvariantCulture), incomingMessage.Id);
            Assert.Equal(message.Time, incomingMessage.Time);
            Assert.Equal(message.From.Id.ToString(CultureInfo.InvariantCulture), incomingMessage.From.Id);
            Assert.Equal(message.Text, incomingMessage.Text);
            Assert.Equal(Consts.ChannelId, incomingMessage.MessengerId);
            Assert.Null(incomingMessage.CallbackData);
        }

        [Fact]
        public void Map_CallbackQuery_IncomingMessage()
        {
            //arrange
            var fixture = new Fixture();
            var callbackQuery = new Telegram.Models.CallbackQuery()
            {
                Id = fixture.Create<string>(),
                From = fixture.Create<User>(),
                CallbackData = fixture.Create<string>(),
                ChatInstance = fixture.Create<string>(),
                Message = new Telegram.Models.Message()
                {
                    Chat = fixture.Create<Chat>()
                }
            };
            var attachmentMapper = new IncomingAttachmentMapperMock();
            var mapper = new IncomingMessageMapper(attachmentMapper.Object);

            //act
            var incomingMessage = mapper.Map(callbackQuery);

            //assert
            attachmentMapper.VerifyNoOtherCalls();
            Assert.Empty(incomingMessage.Attachments);
            Assert.Equal(callbackQuery.Id.ToString(CultureInfo.InvariantCulture), incomingMessage.Id);
            Assert.Equal(default, incomingMessage.Time);
            Assert.Equal(callbackQuery.From.Id.ToString(CultureInfo.InvariantCulture), incomingMessage.From.Id);
            Assert.Null(incomingMessage.Text);
            Assert.Equal(Consts.ChannelId, incomingMessage.MessengerId);
            Assert.Equal(callbackQuery.CallbackData, incomingMessage.CallbackData);
        }

        [Fact]
        public void Map_PrivateMessage_IncomingMessage()
        {
            //arrange
            var fixture = new Fixture();
            var message = new Telegram.Models.Message()
            {
                Id = fixture.Create<int>(),
                Caption = fixture.Create<string>(),
                Chat = fixture.Build<Chat>().With(chat => chat.Type, ChatType.Private).Create(),
                From = fixture.Create<User>(),
                Text = fixture.Create<string>()
            };
            var attachmentMapper = new IncomingAttachmentMapperMock();
            var mapper = new IncomingMessageMapper(attachmentMapper.Object);

            //act
            var incomingMessage = mapper.Map(message);

            //assert
            Assert.Equal(message.From.Id.ToString(CultureInfo.InvariantCulture), incomingMessage.From.Id);
            Assert.Null(incomingMessage.Group);
        }

        [Fact]
        public void Map_GroupMessage_IncomingMessage()
        {
            //arrange
            var fixture = new Fixture();
            var message = new Telegram.Models.Message()
            {
                Id = fixture.Create<int>(),
                Caption = fixture.Create<string>(),
                Chat = fixture.Build<Chat>().With(chat => chat.Type, ChatType.Group).Create(),
                From = fixture.Create<User>(),
                Text = fixture.Create<string>()
            };
            var attachmentMapper = new IncomingAttachmentMapperMock();
            var mapper = new IncomingMessageMapper(attachmentMapper.Object);

            //act
            var incomingMessage = mapper.Map(message);

            //assert
            Assert.Equal(message.From.Id.ToString(CultureInfo.InvariantCulture), incomingMessage.From.Id);
            Assert.Equal(message.Chat.Id.ToString(CultureInfo.InvariantCulture), incomingMessage.Group.Id);
        }
    }
}