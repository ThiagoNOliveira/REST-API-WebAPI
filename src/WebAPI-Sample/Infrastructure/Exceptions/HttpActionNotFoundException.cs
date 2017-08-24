using System;

namespace WebApiSample.Infrastructure.Exceptions
{
    public class HttpActionNotFoundException : Exception
    {
        public HttpActionNotFoundException(string message) : base(message)
        {
        }
    }
}