using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Artice.Core.AspNetCore;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host.Executors;
using Microsoft.Extensions.Options;

namespace Artice.AspNetCore.AzureWebJob
{
    public class LongPollingJobHost : JobHost
    {
        private readonly IEnumerable<ILongPollingProcessor> _processors;

        public LongPollingJobHost(
            IOptions<JobHostOptions> options,
            IJobHostContextFactory jobHostContextFactory,
            IEnumerable<ILongPollingProcessor> processors)
            : base(options, jobHostContextFactory)
        {
            _processors = processors;
        }

        protected override Task StartAsyncCore(CancellationToken cancellationToken)
        {
            foreach (var longPollingProcessor in _processors)
            {
                longPollingProcessor.StartRequesting();
            }

            return base.StartAsyncCore(cancellationToken);
        }

        protected override Task StopAsyncCore(CancellationToken cancellationToken)
        {
            foreach (var longPollingProcessor in _processors)
            {
                longPollingProcessor.StopRequesting();
            }

            return base.StopAsyncCore(cancellationToken);
        }

        protected override void Dispose(bool disposing)
        {
            foreach (var processor in _processors)
            {
                processor.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}