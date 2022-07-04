using Artice.Core.Models;

namespace Artice.Core.AspNetCore.Models
{
    public class WebhookProcessingResult
    {
        public IncomingMessage IncomingMessage { get; set; }
        public WebhookResponse Response { get; set; }
    }
}
