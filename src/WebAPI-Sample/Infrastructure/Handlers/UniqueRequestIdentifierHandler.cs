using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace WebApiSample.Infrastructure.Handlers
{
    public class UniqueRequestIdentifierHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            if (HttpContext.Current == null) return base.SendAsync(request, cancellationToken);

            var requestId = HttpContext.Current.Request.Headers["X-RequestId"];

            if (string.IsNullOrWhiteSpace(requestId))
                requestId = Guid.NewGuid().ToString();

            HttpContext.Current.Items.Add("RequestId", requestId);

            return base.SendAsync(request, cancellationToken);
        }
    }
}