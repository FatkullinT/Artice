using System;
using System.Linq.Expressions;
using Artice.Core.Models;
using Artice.Telegram.Mapping;
using Artice.Testing.Core;
using Moq;

namespace Artice.Telegram.Tests.Mocks
{
    public class OutgoingMessageMapperMock : BaseMock<IOutgoingMessageMapper, OutgoingMessageMapperMock>
    {
        public void VerifyObjectOnMap(Expression<Func<Keyboard, bool>> match)
        {
            VerifyObjectOnMap(match, Times.Once());
        }

        public void VerifyObjectOnMap(Expression<Func<Keyboard, bool>> match, Times times)
        {
            Verify(mapper => mapper.Map(It.Is(match)), times);
        }
    }
}