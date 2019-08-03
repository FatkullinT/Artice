using Artice.Core.Models;

namespace Artice.Vk.Mapping
{
    public interface IIncomingAttachmentMapper
    {
        Attachment Map(Models.Attachment src);
        Sticker Map(Models.Sticker src);
        Document Map(Models.Document src);
        Audio Map(Models.Audio src);
        Video Map(Models.Video src);
        Image Map(Models.Photo src);
    }
}