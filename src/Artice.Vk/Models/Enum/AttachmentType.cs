using System.Runtime.Serialization;

namespace Artice.Vk.Models.Enum
{
    public enum AttachmentType
    {
        [EnumMember(Value = AttachmentTypeNames.Photo)]
        Photo,

        [EnumMember(Value = AttachmentTypeNames.Video)]
        Video,

        [EnumMember(Value = AttachmentTypeNames.Audio)]
        Audio,

        [EnumMember(Value = AttachmentTypeNames.AudioMessage)]
        AudioMessage,

        [EnumMember(Value = AttachmentTypeNames.Document)]
        Document,

        [EnumMember(Value = AttachmentTypeNames.Link)]
        Link,

        [EnumMember(Value = AttachmentTypeNames.Market)]
        Market,

        [EnumMember(Value = AttachmentTypeNames.MarketAlbum)]
        MarketAlbum,

        [EnumMember(Value = AttachmentTypeNames.Wall)]
        Wall,

        [EnumMember(Value = AttachmentTypeNames.WallReply)]
        WallReply,

        [EnumMember(Value = AttachmentTypeNames.Sticker)]
        Sticker,

        [EnumMember(Value = AttachmentTypeNames.Gift)]
        Gift
    }
}