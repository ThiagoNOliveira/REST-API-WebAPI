using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public string Put(Dictionary<string, object> product)
        {
            var validator = new Validator<Product>();

            List<string> erros;
            if (!validator.Validate(product.Keys.ToArray(), out erros))
                throw new ValidationException(erros.ToString());

            return "Ok";
        }
    }
}