using System;

namespace Artice.Core.AspNetCore
{
    public interface ILongPollingProcessor : IDisposable
    {
        void StartRequesting();

        void StopRequesting();
    }
}