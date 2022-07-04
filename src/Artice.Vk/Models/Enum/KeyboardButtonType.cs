using System.Runtime.Serialization;

namespace Artice.Vk.Models.Enum
{
    public enum KeyboardButtonType
    {
        [EnumMember(Value = "text")]
        Text,

        [EnumMember(Value = "location")]
        Location,

        [EnumMember(Value = "vkpay")]
        VkPay,

        [EnumMember(Value = "open_app")]
        VkApps
    }
}