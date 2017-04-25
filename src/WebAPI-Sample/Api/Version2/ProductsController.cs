using System.Web.Http.Cors;
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
    }
}