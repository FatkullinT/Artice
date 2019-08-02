using System.Linq;
using Artice.Core.Models;
using Artice.Telegram.Mapping;
using AutoFixture;
using Xunit;

namespace Artice.Telegram.Tests
{
    public class OutgoingMessageMapperTests
    {
        [Fact]
        public void Map_InlineKeyboard_ReplyMarkup()
        {
            //arrange
            var fixture = new Fixture();
            var keyboard = fixture.Create<Keyboard>();
            var mapper = new OutgoingMessageMapper();
            
            //act
            var markup = mapper.Map(keyboard);

            //assert
            Assert.Equal(keyboard.Buttons.Count, markup.InlineKeyboard.Sum(buttons => buttons.Length));
            foreach (var button in keyboard.Buttons)
            {
                Assert.Contains(markup.InlineKeyboard.SelectMany(buttons => buttons), 
                    b => b.ButtonText == button.ButtonText 
                         && b.CallbackData == button.CallbackData);
            }
        }
    }
}