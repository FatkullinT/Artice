using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Artice.Core.AspNetCore;
using Microsoft.Extensions.Hosting;

namespace Artice.AspNetCore.OnPremises
{
    public class LongPollingService : IHostedService, IDisposable
    {
        private readonly IEnumerable<ILongPollingProcessor> _processors;

        public LongPollingService(IEnumerable<ILongPollingProcessor> processors)
        {
            _processors = processors;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            foreach (var longPollingProcessor in _processors)
            {
                longPollingProcessor.StartRequesting();
            }
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            foreach (var longPollingProcessor in _processors)
            {
                longPollingProcessor.StopRequesting();
            }
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            foreach (var processor in _processors)
            {
                processor.Dispose();
            }
        }
    }
}