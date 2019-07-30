using System.Collections.Generic;
using System.Linq;
using Artice.Core.Models;
using Artice.Core.Models.Files;
using Artice.Telegram.Files;
using Artice.Telegram.Models;
using Artice.Telegram.Models.ReplyMarkups;
using Artice.Telegram.Tests.Mocks;
using AutoFixture;
using Moq;
using Xunit;
using Audio = Artice.Core.Models.Audio;
using Document = Artice.Core.Models.Document;
using Message = Artice.Telegram.Models.Message;
using Sticker = Artice.Core.Models.Sticker;
using Video = Artice.Core.Models.Video;

namespace Artice.Telegram.Tests
{
    public class TelegramOutgoingMessageProviderTests
    {
        [Fact]
        public async void SendMessageAsync_TextMessage_MessageSent()
        {
            //arrange
            var fixture = new Fixture();

            var message = fixture.Build<OutgoingMessage>()
                .Without(outgoingMessage => outgoingMessage.Attachments)
                .Create();

            var telegramKeyboardMarkup = fixture.Build<InlineKeyboardMarkup>().Create();
            var mapper = new OutgoingMessageMapperMock().Returns(telegramKeyboardMarkup);
            var attachmentMapper = new IncomingAttachmentMapperMock();
            var clientMock = new TelegramHttpClientMock();

            var provider = new TelegramOutgoingMessageProvider(
                mapper.Object,
                attachmentMapper.Object,
                () => clientMock.Object);

            //act
            await provider.SendMessageAsync(message);

            //assert
            mapper.VerifyObjectOnMap(keyboard => ReferenceEquals(keyboard, message.InlineKeyboard));
            clientMock.VerifyPost<Telegram.Models.Message>("sendMessage",
                parameters => (string)parameters["chat_id"] == message.Group.Id
                              && (string)parameters["text"] == message.Text
                              && ReferenceEquals(parameters["reply_markup"], telegramKeyboardMarkup));
            mapper.VerifyNoOtherCalls();
            clientMock.VerifyNoOtherCalls();
        }


        [Fact]
        public async void SendMessageAsync_ImageGroupAttachment_MessageSent()
        {
            //arrange
            var fixture = new Fixture();
            var attachments1 = fixture
                .Build<Image>()
                .With(image => image.File, () => fixture.Create<OutgoingLocalFile>())
                .CreateMany(3)
                .Select(image => (Attachment)image)
                .ToArray();

            var attachments2 = fixture
                .Build<Image>()
                .With(image => image.File, () => fixture.Create<OutgoingFileByUri>())
                .CreateMany(1)
                .Select(image => (Attachment)image)
                .ToArray();

            var message = fixture.Build<OutgoingMessage>()
                .With(outgoingMessage => outgoingMessage.Attachments,
                    attachments1)
                .Create();

            var telegramKeyboardMarkup = fixture.Build<InlineKeyboardMarkup>().Create();
            var mapper = new OutgoingMessageMapperMock().Returns(telegramKeyboardMarkup);
            var attachmentMapper = new IncomingAttachmentMapperMock().Returns((IEnumerable<Attachment>)attachments2);
            var returnedMessage = fixture
                .Build<Message>()
                .Without(m => m.ReplyToMessage)
                .Without(m => m.PinnedMessage)
                .With(m => m.Photo, fixture.CreateMany<PhotoSize>(3).ToArray)
                .Create();
            var clientMock = new TelegramHttpClientMock()
                .ReturnsAsync(new ApiResponse<Message>() { Ok = true, ResultObject = returnedMessage });

            var provider = new TelegramOutgoingMessageProvider(
                mapper.Object,
                attachmentMapper.Object,
                () => clientMock.Object);

            //act
            await provider.SendMessageAsync(message);

            //assert
            mapper.VerifyObjectOnMap(keyboard => ReferenceEquals(keyboard, message.InlineKeyboard));
            clientMock.VerifyPostFiles<Artice.Telegram.Models.Message>("sendPhoto",
                parameters => parameters["chat_id"] == message.Group.Id
                              && parameters.ContainsKey("caption")
                              && parameters["caption"] == message.Text
                              && parameters.ContainsKey("reply_markup")
                              && !string.IsNullOrEmpty(parameters["reply_markup"]),
                files => files != null && files.Count() == 1 && files.First().Key == "photo" && files.First().Value is OutgoingLocalFile);

            clientMock.VerifyPostFiles<Telegram.Models.Message>("sendPhoto",
                parameters => parameters["chat_id"] == message.Group.Id
                              && !parameters.ContainsKey("caption")
                              && !parameters.ContainsKey("reply_markup"),
                files => files != null && files.Count() == 1 && files.First().Key == "photo" && files.First().Value is OutgoingLocalFile,
                Times.Exactly(2));

            foreach (var messageAttachment in message.Attachments)
            {
                var multiTypeFile = Assert.IsType<OutgoingMultiTypeFile>(messageAttachment.File);
                Assert.Equal(2, multiTypeFile.InnerFiles.Count);
            }

            mapper.VerifyNoOtherCalls();
            clientMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void SendMessageAsync_ImageAttachment_MessageSent()
        {
            //arrange
            var fixture = new Fixture();
            var attachments1 = fixture
                .Build<Image>()
                .With(image => image.File, () => fixture.Create<OutgoingLocalFile>())
                .CreateMany(1)
                .Select(image => (Attachment)image)
                .ToArray();

            var attachments2 = fixture
                .Build<Image>()
                .With(image => image.File, () => fixture.Create<OutgoingFileByUri>())
                .CreateMany(1)
                .Select(image => (Attachment)image)
                .ToArray();

            var message = fixture.Build<OutgoingMessage>()
                .With(outgoingMessage => outgoingMessage.Attachments,
                    attachments1)
                .Create();

            var telegramKeyboardMarkup = fixture.Build<InlineKeyboardMarkup>().Create();
            var mapper = new OutgoingMessageMapperMock().Returns(telegramKeyboardMarkup);
            var attachmentMapper = new IncomingAttachmentMapperMock().Returns((IEnumerable<Attachment>)attachments2);
            var returnedMessage = fixture
                .Build<Message>()
                .Without(m => m.ReplyToMessage)
                .Without(m => m.PinnedMessage)
                .Create();
            var clientMock = new TelegramHttpClientMock()
                .ReturnsAsync(new ApiResponse<Message>() { Ok = true, ResultObject = returnedMessage });

            var provider = new TelegramOutgoingMessageProvider(
                mapper.Object,
                attachmentMapper.Object,
                () => clientMock.Object);

            //act
            await provider.SendMessageAsync(message);

            //assert
            mapper.VerifyObjectOnMap(keyboard => ReferenceEquals(keyboard, message.InlineKeyboard));
            clientMock.VerifyPostFiles<Artice.Telegram.Models.Message>("sendPhoto",
                parameters => parameters["chat_id"] == message.Group.Id
                              && parameters.ContainsKey("caption")
                              && parameters["caption"] == message.Text
                              && parameters.ContainsKey("reply_markup")
                              && !string.IsNullOrEmpty(parameters["reply_markup"]),
                files => files != null && files.Count() == 1 && files.First().Key == "photo" && files.First().Value is OutgoingLocalFile);
            var multiTypeFile = Assert.IsType<OutgoingMultiTypeFile>(message.Attachments.Single().File);
            Assert.Equal(2, multiTypeFile.InnerFiles.Count);
            mapper.VerifyNoOtherCalls();
            clientMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void SendMessageAsync_VideoAttachment_MessageSent()
        {
            //arrange
            var fixture = new Fixture();
            var attachments1 = fixture
                .Build<Video>()
                .With(image => image.File, () => fixture.Create<OutgoingLocalFile>())
                .CreateMany(1)
                .Select(image => (Attachment)image)
                .ToArray();

            var attachments2 = fixture
                .Build<Video>()
                .With(image => image.File, () => fixture.Create<OutgoingFileByUri>())
                .CreateMany(1)
                .Select(image => (Attachment)image)
                .ToArray();

            var message = fixture.Build<OutgoingMessage>()
                .With(outgoingMessage => outgoingMessage.Attachments,
                    attachments1)
                .Create();

            var telegramKeyboardMarkup = fixture.Build<InlineKeyboardMarkup>().Create();
            var mapper = new OutgoingMessageMapperMock().Returns(telegramKeyboardMarkup);
            var attachmentMapper = new IncomingAttachmentMapperMock().Returns((IEnumerable<Attachment>)attachments2);
            var returnedMessage = fixture
                .Build<Message>()
                .Without(m => m.ReplyToMessage)
                .Without(m => m.PinnedMessage)
                .Create();
            var clientMock = new TelegramHttpClientMock()
                .ReturnsAsync(new ApiResponse<Message>() { Ok = true, ResultObject = returnedMessage });

            var provider = new TelegramOutgoingMessageProvider(
                mapper.Object,
                attachmentMapper.Object,
                () => clientMock.Object);

            //act
            await provider.SendMessageAsync(message);

            //assert
            mapper.VerifyObjectOnMap(keyboard => ReferenceEquals(keyboard, message.InlineKeyboard));
            clientMock.VerifyPostFiles<Artice.Telegram.Models.Message>("sendVideo",
                parameters => parameters["chat_id"] == message.Group.Id
                              && parameters.ContainsKey("caption")
                              && parameters["caption"] == message.Text
                              && parameters.ContainsKey("reply_markup")
                              && !string.IsNullOrEmpty(parameters["reply_markup"]),
                files => files != null && files.Count() == 1 && files.First().Key == "video" && files.First().Value is OutgoingLocalFile);
            var multiTypeFile = Assert.IsType<OutgoingMultiTypeFile>(message.Attachments.Single().File);
            Assert.Equal(2, multiTypeFile.InnerFiles.Count);
            mapper.VerifyNoOtherCalls();
            clientMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void SendMessageAsync_AudioAttachment_MessageSent()
        {
            //arrange
            var fixture = new Fixture();
            var attachments1 = fixture
                .Build<Audio>()
                .With(image => image.File, () => fixture.Create<OutgoingLocalFile>())
                .CreateMany(1)
                .Select(image => (Attachment)image)
                .ToArray();

            var attachments2 = fixture
                .Build<Audio>()
                .With(image => image.File, () => fixture.Create<OutgoingFileByUri>())
                .CreateMany(1)
                .Select(image => (Attachment)image)
                .ToArray();

            var message = fixture.Build<OutgoingMessage>()
                .With(outgoingMessage => outgoingMessage.Attachments,
                    attachments1)
                .Create();

            var telegramKeyboardMarkup = fixture.Build<InlineKeyboardMarkup>().Create();
            var mapper = new OutgoingMessageMapperMock().Returns(telegramKeyboardMarkup);
            var attachmentMapper = new IncomingAttachmentMapperMock().Returns((IEnumerable<Attachment>)attachments2);
            var returnedMessage = fixture
                .Build<Message>()
                .Without(m => m.ReplyToMessage)
                .Without(m => m.PinnedMessage)
                .Create();
            var clientMock = new TelegramHttpClientMock()
                .ReturnsAsync(new ApiResponse<Message>() { Ok = true, ResultObject = returnedMessage });

            var provider = new TelegramOutgoingMessageProvider(
                mapper.Object,
                attachmentMapper.Object,
                () => clientMock.Object);

            //act
            await provider.SendMessageAsync(message);

            //assert
            mapper.VerifyObjectOnMap(keyboard => ReferenceEquals(keyboard, message.InlineKeyboard));
            clientMock.VerifyPostFiles<Artice.Telegram.Models.Message>("sendAudio",
                parameters => parameters["chat_id"] == message.Group.Id
                              && parameters.ContainsKey("caption")
                              && parameters["caption"] == message.Text
                              && parameters.ContainsKey("reply_markup")
                              && !string.IsNullOrEmpty(parameters["reply_markup"]),
                files => files != null && files.Count() == 1 && files.First().Key == "audio" && files.First().Value is OutgoingLocalFile);
            var multiTypeFile = Assert.IsType<OutgoingMultiTypeFile>(message.Attachments.Single().File);
            Assert.Equal(2, multiTypeFile.InnerFiles.Count);
            mapper.VerifyNoOtherCalls();
            clientMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void SendMessageAsync_DocumentAttachment_MessageSent()
        {
            //arrange
            var fixture = new Fixture();
            var attachments1 = fixture
                .Build<Document>()
                .With(image => image.File, () => fixture.Create<OutgoingLocalFile>())
                .CreateMany(1)
                .Select(image => (Attachment)image)
                .ToArray();

            var attachments2 = fixture
                .Build<Document>()
                .With(image => image.File, () => fixture.Create<OutgoingFileByUri>())
                .CreateMany(1)
                .Select(image => (Attachment)image)
                .ToArray();

            var message = fixture.Build<OutgoingMessage>()
                .With(outgoingMessage => outgoingMessage.Attachments,
                    attachments1)
                .Create();

            var telegramKeyboardMarkup = fixture.Build<InlineKeyboardMarkup>().Create();
            var mapper = new OutgoingMessageMapperMock().Returns(telegramKeyboardMarkup);
            var attachmentMapper = new IncomingAttachmentMapperMock().Returns((IEnumerable<Attachment>)attachments2);
            var returnedMessage = fixture
                .Build<Message>()
                .Without(m => m.ReplyToMessage)
                .Without(m => m.PinnedMessage)
                .Create();
            var clientMock = new TelegramHttpClientMock()
                .ReturnsAsync(new ApiResponse<Message>() { Ok = true, ResultObject = returnedMessage });

            var provider = new TelegramOutgoingMessageProvider(
                mapper.Object,
                attachmentMapper.Object,
                () => clientMock.Object);

            //act
            await provider.SendMessageAsync(message);

            //assert
            mapper.VerifyObjectOnMap(keyboard => ReferenceEquals(keyboard, message.InlineKeyboard));
            clientMock.VerifyPostFiles<Artice.Telegram.Models.Message>("sendDocument",
                parameters => parameters["chat_id"] == message.Group.Id
                              && parameters.ContainsKey("caption")
                              && parameters["caption"] == message.Text
                              && parameters.ContainsKey("reply_markup")
                              && !string.IsNullOrEmpty(parameters["reply_markup"]),
                files => files != null && files.Count() == 1 && files.First().Key == "document" && files.First().Value is OutgoingLocalFile);
            var multiTypeFile = Assert.IsType<OutgoingMultiTypeFile>(message.Attachments.Single().File);
            Assert.Equal(2, multiTypeFile.InnerFiles.Count);
            mapper.VerifyNoOtherCalls();
            clientMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void SendMessageAsync_StickerAttachment_MessageSent()
        {
            //arrange
            var fixture = new Fixture();
            var attachments1 = fixture
                .Build<Sticker>()
                .With(image => image.File, () => fixture.Create<OutgoingLocalFile>())
                .CreateMany(1)
                .Select(image => (Attachment)image)
                .ToArray();

            var attachments2 = fixture
                .Build<Sticker>()
                .With(image => image.File, () => fixture.Create<OutgoingFileByUri>())
                .CreateMany(1)
                .Select(image => (Attachment)image)
                .ToArray();

            var message = fixture.Build<OutgoingMessage>()
                .With(outgoingMessage => outgoingMessage.Attachments,
                    attachments1)
                .Create();

            var telegramKeyboardMarkup = fixture.Build<InlineKeyboardMarkup>().Create();
            var mapper = new OutgoingMessageMapperMock().Returns(telegramKeyboardMarkup);
            var attachmentMapper = new IncomingAttachmentMapperMock().Returns((IEnumerable<Attachment>)attachments2);
            var returnedMessage = fixture
                .Build<Message>()
                .Without(m => m.ReplyToMessage)
                .Without(m => m.PinnedMessage)
                .Create();
            var clientMock = new TelegramHttpClientMock()
                .ReturnsAsync(new ApiResponse<Message>() { Ok = true, ResultObject = returnedMessage });

            var provider = new TelegramOutgoingMessageProvider(
                mapper.Object,
                attachmentMapper.Object,
                () => clientMock.Object);

            //act
            await provider.SendMessageAsync(message);

            //assert
            mapper.VerifyObjectOnMap(keyboard => ReferenceEquals(keyboard, message.InlineKeyboard));
            clientMock.VerifyPostFiles<Artice.Telegram.Models.Message>("sendSticker",
                parameters => parameters["chat_id"] == message.Group.Id
                              && parameters.ContainsKey("caption")
                              && parameters["caption"] == message.Text
                              && parameters.ContainsKey("reply_markup")
                              && !string.IsNullOrEmpty(parameters["reply_markup"]),
                files => files != null && files.Count() == 1 && files.First().Key == "sticker" && files.First().Value is OutgoingLocalFile);
            var multiTypeFile = Assert.IsType<OutgoingMultiTypeFile>(message.Attachments.Single().File);
            Assert.Equal(2, multiTypeFile.InnerFiles.Count);
            mapper.VerifyNoOtherCalls();
            clientMock.VerifyNoOtherCalls();
        }
    }
}