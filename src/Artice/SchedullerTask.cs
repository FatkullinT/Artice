using System;
using System.Threading.Tasks;
using Artice.Context;

namespace Artice
{
    public class SchedullerTask
    {
        public DateTime StartTime { get; set; }

        public TimeSpan Interval { get; set; }

        public Func<ContextStorage, Task> Handler { get; set; }
    }
}