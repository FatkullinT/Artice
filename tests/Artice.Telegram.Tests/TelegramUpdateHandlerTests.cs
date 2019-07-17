using Artice.Telegram.MapConfig;
using Artice.Telegram.Models;
using AutoFixture;
using AutoMapper;
using Xunit;

namespace Artice.Telegram.Tests
{
	public class TelegramUpdateHandlerTests
	{
		[Fact]
		public void Convert_TextMessage_IncomingMessageWithText()
		{
			//arrange
			var fixture = new Fixture();
			IMapper mapper = new Mapper(new MapperConfiguration(expression => expression.AddProfile(new TelegramMapProfile())));
			var handler = new TelegramUpdateHandler(mapper);
			var update = new Update()
			{
				Message = fixture.Build<Message>()
					.Without(m => m.ReplyToMessage)
					.Without(m => m.PinnedMessage)
					.Create()
			};

			//act
			var message = handler.Handle(update);

			//assert
			Assert.Equal(Consts.TelegramId, message.MessengerId);
			Assert.Equal(update.Message.Text, message.Text);
			Assert.Equal(update.Message.From.Id, int.Parse(message.From.Id));
			Assert.Equal(update.Message.Chat.Id, int.Parse(message.Chat.Id));
			Assert.Equal(update.Message.Time, message.Time);
		}

		[Fact]
		public void Convert_Callback_IncomingMessageWithCommand()
		{
			//arrange
			var fixture = new Fixture();
			IMapper mapper = new Mapper(new MapperConfiguration(expression => expression.AddProfile(new TelegramMapProfile())));
			var handler = new TelegramUpdateHandler(mapper);
			var update = new Update()
			{
				CallbackQuery = fixture.Build<CallbackQuery>()
					.Without(query => query.Message)
					.Create()
			};

			//act
			var message = handler.Handle(update);

			//assert
			Assert.Equal(Consts.TelegramId, message.MessengerId);
			Assert.Equal(update.CallbackQuery.CallbackData, message.CallbackData);
			Assert.Equal(update.CallbackQuery.From.Id, int.Parse(message.From.Id));
		}
	}
}