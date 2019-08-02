using Artice.Core.Models;

namespace Artice.Vk.Mapping
{
    public interface IOutgoingMessageMapper
    {
        Vk.Models.Keyboard Map(Keyboard src);
    }
}