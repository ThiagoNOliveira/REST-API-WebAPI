using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using WebApiSample.Infrastructure;
using WebApiSample.Models.Entities;
using WebApiSample.Models.Repository;

namespace WebApiSample.Api.Version2
{
    /// <summary>
    ///     Example Two of RestFULL Resource
    /// </summary>
    /// <remarks>
    ///     ExampleTwoController version 1 remarks
    /// </remarks>
    [EnableCors("*", "*", "*")]
    public class ProductsController : Version1.ProductsController
    {
        /// <summary>
        ///     ProductsController version 1 'GET' method description
        /// </summary>
        /// <returns>Description of return value</returns>
        public override Product Get(long id)
        {
            return Products.Get(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> Put(Dictionary<string, object> product)
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}