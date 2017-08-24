using System.Net;
using System.Web.Http;
using System.Web.Http.Controllers;
using WebApiSample.Controllers.Version1;

namespace WebApiSample.Infrastructure
{
    public class HttpNotFoundAwareControllerActionSelector : ApiControllerActionSelector
    {
        public override HttpActionDescriptor SelectAction(HttpControllerContext controllerContext)
        {
            HttpActionDescriptor decriptor;

            try
            {
                decriptor = base.SelectAction(controllerContext);
            }
            catch (HttpResponseException ex)
            {
                var code = ex.Response.StatusCode;

                if (code != HttpStatusCode.NotFound && code != HttpStatusCode.MethodNotAllowed) throw;

                var routeData = controllerContext.RouteData;

                routeData.Values["action"] = "Handle405";

                IHttpController httpController = new ErrorController();

                controllerContext.Controller = httpController;
                controllerContext.ControllerDescriptor = new HttpControllerDescriptor(controllerContext.Configuration,
                    "Error", httpController.GetType());

                decriptor = base.SelectAction(controllerContext);
            }
            return decriptor;
        }
    }
}