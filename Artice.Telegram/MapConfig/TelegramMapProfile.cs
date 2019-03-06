using System.Linq;
using Artice.Core.Models;
using Artice.Telegram.Models;
using Artice.Telegram.Models.ReplyMarkups;
using AutoMapper;
using Chat = Artice.Telegram.Models.Chat;
using KeyboardButton = Artice.Core.Models.KeyboardButton;
using Message = Artice.Telegram.Models.Message;
using User = Artice.Telegram.Models.User;

namespace Artice.Telegram.MapConfig
{
    public class TelegramMapProfile : Profile
    {
        public TelegramMapProfile()
        {
            CreateMap<Message, IncomingMessage>()
                .ForMember(dest => dest.Attachments, method => method.MapFrom(src => src))
                .ForMember(dest => dest.CallbackData, method => method.Ignore());

            CreateMap<CallbackQuery, IncomingMessage>()
                .ForMember(dest => dest.Time, method => method.Ignore())
                .ForMember(dest => dest.Attachments, method => method.Ignore())
                .ForMember(dest => dest.Text, method => method.Ignore())
                .ForMember(dest => dest.Chat,
                    method => method.MapFrom(src => new Core.Models.Chat {Id = src.From.Id.ToString()}));

            CreateMap<Chat, Core.Models.Chat>();

            CreateMap<User, Core.Models.User>();

            CreateMap<Message, Attachment[]>().ConvertUsing<TelegramAttachmentMap>();

            CreateMap<InlineKeyboard, InlineKeyboardMarkup>()
                .ForMember(dest => dest.InlineKeyboard,
                    method =>
                        method.MapFrom(
                            src =>
                                src.Buttons.GroupBy(key => key.RowOrder)
                                    .OrderBy(keyRow => keyRow.Key)
                                    .Select(keyRow => keyRow.OrderBy(key => key.ColumnOrder).ToArray())
                                    .ToArray()));

            CreateMap<KeyboardButton, InlineKeyboardButton>()
                .ForMember(dest => dest.CallbackGame, method => method.Ignore())
                .ForMember(dest => dest.SwitchInlineQuery, method => method.Ignore())
                .ForMember(dest => dest.SwitchInlineQueryCurrentChat, method => method.Ignore());
        }
    }
}