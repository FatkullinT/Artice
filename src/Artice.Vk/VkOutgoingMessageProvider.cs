using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Artice.Core.Models;
using Artice.Core.OutgoingMessages;
using Artice.Vk.HttpClients;

namespace Artice.Vk
{
    public class VkOutgoingMessageProvider : IOutgoingMessageProvider
    {

        private readonly Func<IVkHttpClient> _clientConstructor;

        public string ChannelId => Consts.ChannelId;

        public VkOutgoingMessageProvider(Func<IVkHttpClient> clientConstructor)
        {
            _clientConstructor = clientConstructor;
        }

        public async Task SendMessageAsync(OutgoingMessage message, CancellationToken cancellationToken = new CancellationToken())
        {
            var additionalParameters = new Dictionary<string, string>
            {
                {"random_id", GetRandomId()},
                {"peer_id", message.Group != null ? message.Group.Id : message.To.Id}
            };

            if (message.Group == null)
                additionalParameters.Add("user_id", message.To.Id);

            if (!string.IsNullOrEmpty(message.Text))
                additionalParameters.Add("message", message.Text);

            await _clientConstructor().PostAsync<int>("messages.send", additionalParameters, cancellationToken: cancellationToken);
        }

        private string GetRandomId()
        {
            return DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture);
        }
    }
}