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
    public class GlobalExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            var requestId = HttpContext.Current.Items["RequestId"];
            var message = context.Exception.Message;

            context.Result = new ExceptionResponse
            {
                StatusCode =
                    context.Exception is SecurityException
                        ? HttpStatusCode.Unauthorized
                        : HttpStatusCode.InternalServerError,
                Message = JsonConvert.SerializeObject(new {RequestId = requestId ?? "-", Message = message}),
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