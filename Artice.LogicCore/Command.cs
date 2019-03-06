using System;
using System.Collections.Generic;
using System.Linq;
using Artice.Core.Models;

namespace Artice.LogicCore
{
    public class Command
    {
        public IDictionary<string, string> MessageStrings { get; }

        public IEnumerable<string> ExternalMessageStrings { get; }

        public string CallBackData { get; }

        public IEnumerable<string> ChatBotFilter { get; }

        public Command(params KeyValuePair<string, string>[] messageStrings) : this(messageStrings.ToDictionary(ms=>ms.Key, ms=>ms.Value))
        {
        }

        public Command(IDictionary<string, string> messageStrings, string callBackData = null, string[] externalMessageStrings = null, string[] chatBots = null)
        {
            MessageStrings = messageStrings ?? new Dictionary<string, string>();
            ExternalMessageStrings = externalMessageStrings ?? Enumerable.Empty<string>(); 
            CallBackData = callBackData;
            ChatBotFilter = chatBots;
            if (!MessageStrings.ContainsKey(""))
            {
                MessageStrings.Add("", MessageStrings.Values.Concat(ExternalMessageStrings).FirstOrDefault());
            }
        }

        public string GetDisplayName(string chatBotName)
        {
            return MessageStrings.ContainsKey(chatBotName) ? MessageStrings[chatBotName] : MessageStrings[""];
        }

        public bool IsMatch(IncomingMessage message, string chatBotName)
        {
            if ((ChatBotFilter != null && !ChatBotFilter.Contains(chatBotName)) || message == null)
            {
                return false;
            }
            if (CallBackData != null && !string.IsNullOrEmpty(message.CallbackData) &&
                string.Equals(CallBackData, message.CallbackData))
            {
                return true;
            }
            if (!string.IsNullOrEmpty(message.Text) && MessageStrings.Values.Concat(ExternalMessageStrings).Any(command=>string.Equals(command, message.Text.Trim(), StringComparison.InvariantCultureIgnoreCase)))
            {
                return true;
            }
            return false;
        }
    }
}