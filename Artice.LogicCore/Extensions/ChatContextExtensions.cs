using Artice.LogicCore.Context;

namespace Artice.LogicCore.Extensions
{
    public static class ChatContextExtensions
    {
        private const string CommandHandlerFieldName = "CommandHandler";

        public static void SetCommandHandler(this ChatContext context, CommandHandler handler)
        {
            context.Get<CommandHandler>(CommandHandlerFieldName)?.Delete();
            context.Set(CommandHandlerFieldName, handler);
        }

        public static void RemoveCommandHandler(this ChatContext context)
        {
            context.Get<CommandHandler>(CommandHandlerFieldName)?.Delete();
            context.Set(CommandHandlerFieldName, null);
        }

        public static CommandHandler GetCommandHandler(this ChatContext context)
        {
            return context.Get<CommandHandler>(CommandHandlerFieldName);
        }

        public static TCommandHandler GetCommandHandler<TCommandHandler>(this ChatContext context)
            where TCommandHandler : CommandHandler
        {
            return context.Get<TCommandHandler>(CommandHandlerFieldName);
        }
    }
}