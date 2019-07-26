using System;
using System.Threading;
using Artice.Core.IncomingMessages;
using Microsoft.Extensions.DependencyInjection;

namespace Artice.Core.AspNetCore
{
    public class LongPollingProcessor<TUpdate> : ILongPollingProcessor
    {
        private readonly IInterrogator<TUpdate> _interrogator;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly IServiceProvider _rootServiceProvider;
        //todo: remove to external state storage
        private string _cursor = null;

        public LongPollingProcessor(
            IInterrogator<TUpdate> interrogator,
            IServiceProvider rootServiceProvider)
        {
            _interrogator = interrogator;
            _rootServiceProvider = rootServiceProvider;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public async void StartRequesting()
        {
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                var updateCollection = await _interrogator.GetUpdatesAsync(_cursor, _cancellationTokenSource.Token);
                foreach (var update in updateCollection.Updates)
                {
                    using (var scope = _rootServiceProvider.CreateScope())
                    {
                        var updateHandler = scope.ServiceProvider.GetService<IIncomingUpdateHandler<TUpdate>>();
                        var incomingMessage = await updateHandler.HandleAsync(update, _cancellationTokenSource.Token);

                        var handlers = scope.ServiceProvider.GetServices<IIncomingMessageHandler>();
                        foreach (var handler in handlers)
                        {
                            await handler.Handle(incomingMessage);
                        }
                    }
                }

                _cursor = updateCollection.Cursor;
            }
        }

        public void StopRequesting()
        {
            _cancellationTokenSource.Cancel();
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                _cancellationTokenSource?.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~LongPollingProcessor()
        {
            Dispose(false);
        }
    }
}