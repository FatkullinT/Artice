using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Artice.Core.Models;
using Artice.Core.OutgoingMessages;
using Artice.Vk.HttpClients;
using Artice.Vk.Mapping;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Artice.Vk
{
    public class VkOutgoingMessageProvider : IOutgoingMessageProvider
    {
        private readonly Func<IVkHttpClient> _clientConstructor;
        private readonly IOutgoingMessageMapper _mapper;

        public string ChannelId => Consts.ChannelId;

        public VkOutgoingMessageProvider(Func<IVkHttpClient> clientConstructor, IOutgoingMessageMapper mapper)
        {
            _clientConstructor = clientConstructor;
            _mapper = mapper;
        }

        public async Task SendMessageAsync(OutgoingMessage message, CancellationToken cancellationToken = new CancellationToken())
        {
            var additionalParameters = new Dictionary<string, string>
            {
                {"random_id", GetRandomId()},
                {"peer_id", message.Group != null ? message.Group.Id : message.To.Id}
            };

            if (message.Keyboard != null)
                additionalParameters.Add("keyboard", JsonConvert.SerializeObject(_mapper.Map(message.Keyboard)));

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