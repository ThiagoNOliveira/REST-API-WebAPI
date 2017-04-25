using System.Collections.Generic;
using System.Web.Http;
using WebApiSample.Models.Model;

namespace WebApiSample.Infrastructure
{
    public abstract class BaseApiController : ApiController
    {
        public abstract IEnumerable<dynamic> Get([FromUri] GetModel model);
    }
}