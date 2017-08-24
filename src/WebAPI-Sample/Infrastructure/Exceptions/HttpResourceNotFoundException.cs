using System;

namespace WebApiSample.Infrastructure.Exceptions
{
    public class HttpResourceNotFoundException : Exception
    {
        public HttpResourceNotFoundException(string message) : base(message)
        {
        }
    }
}