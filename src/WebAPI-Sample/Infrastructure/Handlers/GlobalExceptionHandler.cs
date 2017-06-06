using System;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Newtonsoft.Json;

namespace WebApiSample.Infrastructure.Handlers
{
    public class GlobalExceptionHandler : ExceptionHandler, IExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            var requestId = HttpContext.Current.Items["RequestId"];
            var message = context.Exception.Message;
            var exceptionData = context.Exception.Data;

            HttpStatusCode statusCode;

            if (context.Exception is SecurityException)
                statusCode = HttpStatusCode.Unauthorized;
            else if (context.Exception is InvalidOperationException)
                statusCode = HttpStatusCode.BadRequest;
            else
                statusCode = HttpStatusCode.InternalServerError;

            context.Result = new ExceptionResponse
            {
                StatusCode = statusCode,
                Message = JsonConvert.SerializeObject(new { RequestId = requestId ?? "-", Message = message, Errors = exceptionData }),
                Request = context.Request
            };
        }

        public override Task HandleAsync(ExceptionHandlerContext context, CancellationToken cancellationToken)
        {
            Handle(context);

            return base.HandleAsync(context, cancellationToken);
        }

        private class ExceptionResponse : IHttpActionResult
        {
            public HttpStatusCode StatusCode { private get; set; }
            public string Message { private get; set; }
            public HttpRequestMessage Request { private get; set; }

            public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                var response = new HttpResponseMessage(StatusCode)
                {
                    RequestMessage = Request,
                    Content = new StringContent(Message)
                };

                return Task.FromResult(response);
            }
        }
    }
}