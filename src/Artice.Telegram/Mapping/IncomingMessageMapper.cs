using System;
using System.Globalization;
using System.Linq;
using Artice.Core.Models;
using Artice.Telegram.Models.Enums;

namespace Artice.Telegram.Mapping
{
    public class IncomingMessageMapper : IIncomingMessageMapper
    {
        private readonly IIncomingAttachmentMapper _incomingAttachmentMapper;

        public IncomingMessageMapper(IIncomingAttachmentMapper incomingAttachmentMapper)
        {
            _incomingAttachmentMapper = incomingAttachmentMapper;
        }

        public IncomingMessage Map(Telegram.Models.Message message)
        {
            return new IncomingMessage()
            {
                Text = message.Text,
                Time = message.Time,
                Id = MapId(message.Id),
                Group = MapGroup(message.Chat),
                From = Map(message.From),
                MessengerId = Consts.ChannelId,
                Attachments = _incomingAttachmentMapper.Map(message).ToArray()
            };
        }

        public IncomingMessage Map(Telegram.Models.CallbackQuery callbackQuery)
        {
            return new IncomingMessage()
            {
                Id = callbackQuery.Id,
                Group = callbackQuery.Message != null ? MapGroup(callbackQuery.Message.Chat) : null,
                From = Map(callbackQuery.From),
                MessengerId = Consts.ChannelId,
                CallbackData = callbackQuery.CallbackData,
                Attachments = Array.Empty<Attachment>()
            };
        }

        private Group MapGroup(Telegram.Models.Chat src)
        {
            if (src == null || src.Type == ChatType.Private)
                return null;

            return new Group()
            {
                Id = MapId(src.Id)
            };
        }

        private User Map(Telegram.Models.User src)
        {
            if (src == null)
                return null;

            return new User()
            {
                Id = MapId(src.Id)
            };
        }

        private string MapId(long id)
        {
            return id.ToString(CultureInfo.InvariantCulture);
        }
    }
}