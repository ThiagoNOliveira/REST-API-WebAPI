using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebApiSample.Infrastructure.Handlers
{
    public class ResourceOptionsHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Method != HttpMethod.Options) return base.SendAsync(request, cancellationToken);

            var apiExplorer = GlobalConfiguration.Configuration.Services.GetApiExplorer();
            var controllerRequested = request.GetRouteData().Values["controller"] as string;
            var supportedMethods = apiExplorer.ApiDescriptions
                .Where(d =>
                {
                    var controller = d.ActionDescriptor.ControllerDescriptor.ControllerName;
                    return string.Equals(
                        controller, controllerRequested, StringComparison.OrdinalIgnoreCase);
                })
                .Select(d => d.HttpMethod.Method)
                .Distinct();

            var httpMethods = supportedMethods as string[] ?? supportedMethods.ToArray();

            if (!httpMethods.Any())
                return Task.Factory.StartNew(
                    () => request.CreateResponse(HttpStatusCode.NotFound), cancellationToken);

            return Task.Factory.StartNew(() =>
            {
                var resp = new HttpResponseMessage(HttpStatusCode.OK);
                resp.Headers.Add("Access-Control-Allow-Methods", string.Join(",", httpMethods));

                return resp;
            }, cancellationToken);
        }
    }
}