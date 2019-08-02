using Artice.Core.Models;

namespace Artice.Vk.Mapping
{
    public interface IIncomingAttachmentMapper
    {
        Attachment Map(Vk.Models.Attachment src);
    }
}