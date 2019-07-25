using System;
using System.Linq.Expressions;
using Artice.Telegram.Mapping;
using Artice.Telegram.Models;
using Artice.Testing.Core;
using Moq;

namespace Artice.Telegram.Tests.Mocks
{
    public class IncomingMessageMapperMock : BaseMock<IIncomingMessageMapper, IncomingMessageMapperMock>
    {
        public void VerifyMessageOnMap(Expression<Func<Message, bool>> match)
        {
            Verify(mapper => mapper.Map(It.Is(match)), Times.Once);
        }

        public void VerifyCallbackQueryOnMap(Expression<Func<CallbackQuery, bool>> match)
        {
            Verify(mapper => mapper.Map(It.Is(match)), Times.Once);
        }
    }
}