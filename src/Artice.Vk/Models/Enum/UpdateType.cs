using System.Runtime.Serialization;

namespace Artice.Vk.Models.Enum
{
    public enum UpdateType
    {
        [EnumMember(Value = "confirmation")]
        Confirmation,

        [EnumMember(Value = "message_new")]
        NewMessage,

        [EnumMember(Value = "message_reply")]
        MessageReply,

        [EnumMember(Value = "message_edit")]
        MessageEdit,

        [EnumMember(Value = "message_allow")]
        MessageAllow,

        [EnumMember(Value = "message_deny")]
        MessageDeny
    }
}