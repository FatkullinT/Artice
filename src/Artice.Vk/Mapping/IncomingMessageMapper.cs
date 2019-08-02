using System.Globalization;
using System.Linq;
using Artice.Core.Models;
using Artice.Vk.Models;
using Newtonsoft.Json;

namespace Artice.Vk.Mapping
{
    public class IncomingMessageMapper : IIncomingMessageMapper
    {
        private readonly IIncomingAttachmentMapper _incomingAttachmentMapper;

        public IncomingMessageMapper(IIncomingAttachmentMapper incomingAttachmentMapper)
        {
            _incomingAttachmentMapper = incomingAttachmentMapper;
        }

        public IncomingMessage Map(Vk.Models.Message src)
        {
            return new IncomingMessage()
            {
                Id = MapId(src.Id),
                Attachments = src.Attachments.Select(_incomingAttachmentMapper.Map).ToArray(),
                From = new User() { Id = MapId(src.FromId) },
                Group = src.ChatId != src.FromId ? new Group() { Id = MapId(src.ChatId) } : null,
                Text = src.Text,
                CallbackData = src.Payload != null ? JsonConvert.DeserializeObject<Payload>(src.Payload).Command : null,
                Time = src.Date,
                MessengerId = Consts.ChannelId
            };
        }

        private string MapId(long id)
        {
            return id.ToString(CultureInfo.InvariantCulture);
        }
    }
}