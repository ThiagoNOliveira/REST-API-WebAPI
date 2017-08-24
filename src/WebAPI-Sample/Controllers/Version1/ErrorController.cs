using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebApiSample.Infrastructure.Exceptions;

namespace WebApiSample.Controllers.Version1
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : ApiController
    {
        [HttpGet]
        [HttpPost]
        [HttpPut]
        [HttpDelete]
        [HttpHead]
        [HttpOptions]
        [AcceptVerbs("PATCH")]
        public HttpResponseMessage Handle404()
        {
            throw new HttpResourceNotFoundException("The requested resource is not found.");
        }

        [HttpGet]
        [HttpPost]
        [HttpPut]
        [HttpDelete]
        [HttpHead]
        [HttpOptions]
        [AcceptVerbs("PATCH")]
        public HttpResponseMessage Handle405()
        {
            throw new HttpActionNotFoundException("The requested action is not found.");
        }
    }
}