using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Artice.Core.AspNetCore
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
                longPollingProcessor.StartRequesting();
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