using System.Runtime.Serialization;

namespace Artice.Vk.Models.Enum
{
    public enum ActionType
    {
        [EnumMember(Value = "chat_photo_update")]
        ChatPhotoUpdate,

        [EnumMember(Value = "chat_photo_remove")]
        ChatPhotoRemove,

        [EnumMember(Value = "chat_create")]
        ChatCreate,

        [EnumMember(Value = "chat_title_update")]
        ChatTitleUpdate,

        [EnumMember(Value = "chat_invite_user")]
        ChatInviteUser,

        [EnumMember(Value = "chat_kick_user")]
        ChatKickUser,

        [EnumMember(Value = "chat_pin_message")]
        ChatPinMessage,

        [EnumMember(Value = "chat_unpin_message")]
        PhChatUnpinMessage,

        [EnumMember(Value = "chat_invite_user_by_link")]
        ChatInviteUserByLink,
    }
}