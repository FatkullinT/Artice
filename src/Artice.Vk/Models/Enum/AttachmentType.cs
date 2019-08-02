using System.Runtime.Serialization;

namespace Artice.Vk.Models.Enum
{
    public enum AttachmentType
    {
        [EnumMember(Value = "photo")]
        Photo,

        [EnumMember(Value = "video")]
        Video,

        [EnumMember(Value = "audio")]
        Audio,

        [EnumMember(Value = "doc")]
        Document,

        [EnumMember(Value = "link")]
        Link,

        [EnumMember(Value = "market")]
        Market,

        [EnumMember(Value = "market_album")]
        MarketAlbum,

        [EnumMember(Value = "wall")]
        Wall,

        [EnumMember(Value = "wall_reply")]
        WallReply,

        [EnumMember(Value = "sticker")]
        Sticker,

        [EnumMember(Value = "gift")]
        Gift
    }
}