using System;
using System.Threading.Tasks;
using System.Timers;
using Artice.Context;

namespace Artice
{
    public class SchedullerJob
    {
        private Timer _timer;

        private DateTime _scheduledTime;

        private TimeSpan _interval;

        private Func<ContextStorage, Task> _handler;

        private ContextStorage _contextStorage;

        public SchedullerJob(SchedullerTask task, ContextStorage contextStorage)
        {
            _contextStorage = contextStorage;
            _handler = task.Handler;
            _interval = task.Interval;
            _scheduledTime = task.StartTime;
            while (_scheduledTime <= DateTime.UtcNow)
            {
                _scheduledTime = _scheduledTime.Add(_interval);
            }
            _timer = new Timer {Interval = (_scheduledTime - DateTime.UtcNow).TotalMilliseconds};
            _timer.Elapsed += StartJob;
            _timer.Start();
        }

        private void StartJob(object sender, ElapsedEventArgs e)
        {
            _timer.Stop();
            _handler(_contextStorage);
            while (_scheduledTime <= DateTime.UtcNow)
            {
                _scheduledTime = _scheduledTime.Add(_interval);
            }
            _timer.Interval = (_scheduledTime - DateTime.UtcNow).TotalMilliseconds;
            _timer.Start();
        }
    }
}