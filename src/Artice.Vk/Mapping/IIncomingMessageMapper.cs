using Artice.Core.Models;

namespace Artice.Vk.Mapping
{
    public interface IIncomingMessageMapper
    {
        IncomingMessage Map(Vk.Models.Message src);
    }
}