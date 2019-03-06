using System;

namespace Artice.LogicCore.Context.Args
{
    public class ContextDeleteEventArgs : EventArgs
    {
        public ContextDeleteEventArgs(ContextDeleteReason reason)
        {
            Reason = reason;
        }

        public ContextDeleteReason Reason { get; }
    }
}