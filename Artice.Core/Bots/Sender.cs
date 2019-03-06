using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Artice.Core.Models;
using Artice.Core.Queues;
using Microsoft.Extensions.Logging;
using Timer = System.Timers.Timer;

namespace Artice.Core.Bots
{
    public class Sender : IDisposable
    {
        private readonly Func<OutgoingMessage, CancellationToken, Task<bool>> _sendMessageAsync;

        private readonly IMessageQueue _sendingMessageQueue;

        private readonly Timer _timer;

        private OutgoingMessage _currentMessage;

        private readonly int _limit;

        private int _counter;

        private bool _sending;

        private readonly ILogger _logger;

        public Sender(
			IMessageQueue sendingMessageQueue,
			Func<OutgoingMessage,CancellationToken, Task<bool>> sendMessageAsync,
			int limit,
			ILogger logger)
        {
	        _logger = logger;
			_sendingMessageQueue = sendingMessageQueue;
            _sendMessageAsync = sendMessageAsync;
            _limit = limit;
            _counter = 0;
            _timer = new Timer(1000);
            _timer.Elapsed += async(sender, e) => await SendFromQueueAsync(sender, e);
            _timer.Start();

        }

        private async Task SendFromQueueAsync(object sender, ElapsedEventArgs e)
        {
            if (_sending)
            {
                return;
            }
            try
            {
                _sending = true;
                _counter = 0;
                _currentMessage = _currentMessage ?? await _sendingMessageQueue.GetMessageAsync();
                while (_currentMessage != null)
                {
                    if (_counter >= _limit ||
                        !_sendMessageAsync(_currentMessage, new CancellationToken()).GetAwaiter().GetResult())
                    {
                        break;
                    }
                    _counter++;
                    _currentMessage = await _sendingMessageQueue.GetMessageAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, $"QueueName: {_sendingMessageQueue.Name}, Message: {ex.Message}");
            }
            finally
            {
                _sending = false;
            }
        }

        public void Dispose()
        {
            _timer.Stop();
        }
    }
}