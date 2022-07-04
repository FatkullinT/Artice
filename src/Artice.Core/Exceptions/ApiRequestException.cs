using System;
using System.Net;

namespace Artice.Core.Exceptions
{
    public class ApiRequestException : ArticeExecutionException
    {
        public HttpStatusCode StatusCode { get; set; }

        public string ErrorCode { get; set; }

        public ApiRequestException(string message) : base(message)
        { }

        public ApiRequestException(string message, Exception innerException) 
            : base(message, innerException)
        { }
    }
}