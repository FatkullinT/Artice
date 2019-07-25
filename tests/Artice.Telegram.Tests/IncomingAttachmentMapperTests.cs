using System.Linq;
using Artice.Core.Models;
using Artice.Telegram.Files;
using Artice.Telegram.Mapping;
using Artice.Telegram.Tests.Mocks;
using AutoFixture;
using Xunit;

namespace Artice.Telegram.Tests
{
    public class IncomingAttachmentMapperTests
    {
        [Fact]
        public void Map_Photo_ImageAttachment()
        {
            //arrange
            var fixture = new Fixture();
            var message = new Telegram.Models.Message
            {
                Photo = fixture.CreateMany<Telegram.Models.PhotoSize>(3).ToArray()
            };
            var client = new TelegramHttpClientMock();
            var mapper = new IncomingAttachmentMapper(() => client.Object);

            //act
            var markup = mapper.Map(message);

            //assert
            var attachment = Assert.Single(markup);
            Assert.IsType<Image>(attachment);
            Assert.NotNull(attachment.File);
            Assert.Contains(message.Photo, p => ((TelegramFile)attachment.File).FileId == p.FileId);
        }

        [Fact]
        public void Map_Video_VideoAttachment()
        {
            //arrange
            var fixture = new Fixture();
            var message = new Telegram.Models.Message
            {
                Video = fixture.Create<Telegram.Models.Video>()
            };
            var client = new TelegramHttpClientMock();
            var mapper = new IncomingAttachmentMapper(() => client.Object);

            //act
            var markup = mapper.Map(message);

            //assert
            var attachment = Assert.Single(markup);
            Assert.IsType<Video>(attachment);
            Assert.NotNull(attachment.File);
            Assert.Equal(message.Video.FileId, ((TelegramFile)attachment.File).FileId);
        }

        [Fact]
        public void Map_Audio_AudioAttachment()
        {
            //arrange
            var fixture = new Fixture();
            var message = new Telegram.Models.Message
            {
                Audio = fixture.Create<Telegram.Models.Audio>()
            };
            var client = new TelegramHttpClientMock();
            var mapper = new IncomingAttachmentMapper(() => client.Object);

            //act
            var markup = mapper.Map(message);

            //assert
            var attachment = Assert.Single(markup);
            Assert.IsType<Audio>(attachment);
            Assert.NotNull(attachment.File);
            Assert.Equal(message.Audio.FileId, ((TelegramFile)attachment.File).FileId);
        }

        [Fact]
        public void Map_Voice_AudioAttachment()
        {
            //arrange
            var fixture = new Fixture();
            var message = new Telegram.Models.Message
            {
                Voice = fixture.Create<Telegram.Models.Voice>()
            };
            var client = new TelegramHttpClientMock();
            var mapper = new IncomingAttachmentMapper(() => client.Object);

            //act
            var markup = mapper.Map(message);

            //assert
            var attachment = Assert.Single(markup);
            Assert.IsType<Audio>(attachment);
            Assert.NotNull(attachment.File);
            Assert.Equal(message.Voice.FileId, ((TelegramFile)attachment.File).FileId);
        }

        [Fact]
        public void Map_Document_DocumentAttachment()
        {
            //arrange
            var fixture = new Fixture();
            var message = new Telegram.Models.Message
            {
                Document = fixture
                    .Build<Telegram.Models.Document>()
                    .With(d => d.FileName, "testDocument.txt")
                    .Create()
            };
            var client = new TelegramHttpClientMock();
            var mapper = new IncomingAttachmentMapper(() => client.Object);

            //act
            var markup = mapper.Map(message);

            //assert
            var attachment = Assert.Single(markup);
            var document = Assert.IsType<Document>(attachment);
            Assert.Equal(".txt", document.Extention);
            Assert.NotNull(attachment.File);
            Assert.Equal(message.Document.FileId, ((TelegramFile)attachment.File).FileId);
        }

        [Fact]
        public void Map_Sticker_StickerAttachment()
        {
            //arrange
            var fixture = new Fixture();
            var message = new Telegram.Models.Message
            {
                Sticker = fixture.Create<Telegram.Models.Sticker>()
            };
            var client = new TelegramHttpClientMock();
            var mapper = new IncomingAttachmentMapper(() => client.Object);

            //act
            var markup = mapper.Map(message);

            //assert
            var attachment = Assert.Single(markup);
            var sticker = Assert.IsType<Sticker>(attachment);
            Assert.NotNull(attachment.File);
            Assert.Equal(message.Sticker.Emoji, sticker.StickerId);
            Assert.Equal(message.Sticker.FileId, ((TelegramFile)attachment.File).FileId);
        }

        [Fact]
        public void Map_MessageWithoutAttachment_EmptyAttachmentCollection()
        {
            //arrange
            var fixture = new Fixture();
            var message = new Telegram.Models.Message();
            var client = new TelegramHttpClientMock();
            var mapper = new IncomingAttachmentMapper(() => client.Object);

            //act
            var markup = mapper.Map(message);

            //assert
            Assert.Empty(markup);
        }
    }
}