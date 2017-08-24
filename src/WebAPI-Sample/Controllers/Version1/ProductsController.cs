using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApi.OutputCache.V2;
using WebApiSample.Infrastructure;
using WebApiSample.Models.Entities;
using WebApiSample.Models.Model;
using WebApiSample.Models.Repository;
using WebApiSample.Models.Temp;

namespace WebApiSample.Controllers.Version1
{
    /// <summary>
    ///     Example Two of RestFULL Resource
    /// </summary>
    /// <remarks>
    ///     ExampleTwoController version 1 remarks
    /// </remarks>
    [EnableCors("*", "*", "*")]
    public class ProductsController : BaseApiController
    {
        /// <summary>
        ///     ProductsController version 1 'GET' method description
        /// </summary>
        /// <returns>Description of return value</returns>
        [CacheOutput(ClientTimeSpan = 200)]
        public override IEnumerable<dynamic> Get([FromUri] GetModel model)
        {
            if (model?.FilterKey == null)
                return Products.All();

            var filter = Filters.Get(model.FilterKey.Value);

            return Products.Find(filter["Name"].ToString(), filter["Category"].ToString());
        }

        /// <summary>
        ///     ProductsController version 1 'GET' method description
        /// </summary>
        /// <returns>Description of return value</returns>
        [ActionName("GetById")]
        [CacheOutput(ClientTimeSpan = 200, ServerTimeSpan = 200)]
        public virtual Product Get(long id)
        {
            return Products.Get(id);
        }

        /// <summary>
        ///     ProductsController version 1 'POST' method description
        /// </summary>
        /// <returns>Description of return value</returns>
        public virtual void Post([FromBody] Product product)
        {
            Products.Add(product);
        }

        /// <summary>
        ///     ProductsController version 1 'PATCH' method description
        /// </summary>
        /// <returns>Description of return value</returns>
        public virtual void Patch([FromUri] long id, [FromBody] Dictionary<string, object> property)
        {
            var product = Products.Get(id);

            var propertyInfo = product.GetType().GetProperty(property.FirstOrDefault().Key);
            propertyInfo.SetValue(product, Convert.ChangeType(property.FirstOrDefault().Key, propertyInfo.PropertyType),
                null);

            Products.Update(product);
        }

        /// <summary>
        ///     ProductsController version 1 'PUT' method description
        /// </summary>
        /// <returns>Description of return value</returns>
        public virtual void Put([FromUri] long? id, [FromBody] Product product)
        {
            var productObj = Products.Get(id.Value);

            if (id.HasValue && productObj != null)
            {
                Products.Update(product);

                return;
            }

            Products.Add(product);
        }

        /// <summary>
        ///     ProductsController version 1 'DELETE' method description
        /// </summary>
        /// <returns>Description of return value</returns>
        public virtual void Delete(long id)
        {
            Products.Remove(id);
        }
    }
}