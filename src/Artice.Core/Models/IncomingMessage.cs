using System;

namespace Artice.Core.Models
{
    public class IncomingMessage : Message
    {
        public string Id { get;  set; }

        public string MessengerId { get; set; }

        public User From { get; set; }

        public Group Group { get; set; }

        public DateTime Time { get; set; }

        public string CallbackData { get; set; }
    }
}