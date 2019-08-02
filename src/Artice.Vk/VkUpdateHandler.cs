using System.Threading;
using System.Threading.Tasks;
using Artice.Core.IncomingMessages;
using Artice.Core.Models;
using Artice.Vk.Mapping;
using Artice.Vk.Models;
using Artice.Vk.Models.Enum;

namespace Artice.Vk
{
    public class VkUpdateHandler : IIncomingUpdateHandler<Update>
    {
        private readonly IIncomingMessageMapper _mapper;

        public VkUpdateHandler(IIncomingMessageMapper mapper)
        {
            _mapper = mapper;
        }

        public Task<IncomingMessage> HandleAsync(Update incomingUpdate, CancellationToken cancellationToken = default)
        {
            if (incomingUpdate.Type == UpdateType.NewMessage)
                return Task.FromResult(_mapper.Map(incomingUpdate.Object));

            return Task.FromResult((IncomingMessage)null);
        }
    }
}