using System;

namespace Artice.Core.Exceptions
{
    public class ArticeExecutionException : ApplicationException
    {
        public ArticeExecutionException(string message) : base(message)
        {}

        public ArticeExecutionException(string message, Exception innerException) : base(message, innerException)
        {}

        public string BotApiIdentifier { get; set; }
    }
}