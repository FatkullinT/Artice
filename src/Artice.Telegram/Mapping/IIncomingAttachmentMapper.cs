using System.Collections.Generic;
using Artice.Core.Models;

namespace Artice.Telegram.Mapping
{
    public interface IIncomingAttachmentMapper
    {
        IEnumerable<Attachment> Map(Telegram.Models.Message src);
    }
}