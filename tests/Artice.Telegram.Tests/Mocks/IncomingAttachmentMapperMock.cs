using System;
using System.Linq.Expressions;
using Artice.Telegram.Mapping;
using Artice.Telegram.Models;
using Artice.Testing.Core;
using Moq;

namespace Artice.Telegram.Tests.Mocks
{
    public class IncomingAttachmentMapperMock : BaseMock<IIncomingAttachmentMapper, IncomingAttachmentMapperMock>
    {
        public void VerifyAttachmentOnMap(Expression<Func<Message, bool>> match)
        {
            Verify(mapper => mapper.Map(It.Is(match)), Times.Once);
        }
    }
}