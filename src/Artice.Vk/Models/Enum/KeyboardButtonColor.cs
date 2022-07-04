using System.Runtime.Serialization;

namespace Artice.Vk.Models.Enum
{
    public enum KeyboardButtonColor
    {
        [EnumMember(Value = "primary")]
        Primary,

        [EnumMember(Value = "secondary")]
        Secondary,

        [EnumMember(Value = "negative")]
        Negative,

        [EnumMember(Value = "positive")]
        Positive
    }
}