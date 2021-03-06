﻿using System;
using System.Globalization;
using System.Linq;
using Artice.Core.Models;

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
                Chat = Map(message.Chat),
                From = Map(message.From),
                MessengerId = Consts.TelegramId,
                Attachments = _incomingAttachmentMapper.Map(message).ToArray()
            };
        }

        public IncomingMessage Map(Telegram.Models.CallbackQuery callbackQuery)
        {
            return new IncomingMessage()
            {
                Id = callbackQuery.Id,
                Chat = new Core.Models.Chat { Id = callbackQuery.ChatInstance },
                From = Map(callbackQuery.From),
                MessengerId = Consts.TelegramId,
                CallbackData = callbackQuery.CallbackData,
                Attachments = Array.Empty<Attachment>()
            };
        }

        private Chat Map(Telegram.Models.Chat chat)
        {
            return new Chat()
            {
                Id = MapId(chat.Id)
            };
        }

        private User Map(Telegram.Models.User user)
        {
            return new User()
            {
                Id = MapId(user.Id)
            };
        }

        private string MapId(long id)
        {
            return id.ToString(CultureInfo.InvariantCulture);
        }
    }
}