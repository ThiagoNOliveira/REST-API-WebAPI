using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApiSample.Infrastructure;
using WebApiSample.Models.Model;
using WebApiSample.Models.Temp;

namespace WebApiSample.Api.Version1
{
    /// <summary>
    ///     EFilters of RestFULL Resource
    /// </summary>
    /// <remarks>
    ///     ExampleTwoController version 1 remarks
    /// </remarks>
    [EnableCors("*", "*", "*")]
    public class FiltersController : BaseApiController
    {
        /// <summary>
        ///     Filters controller version 1 'POST' method description
        /// </summary>
        /// <returns>Description of return value</returns>
        public long Post([FromBody] Dictionary<string, object> filterProperties)
        {
            return Filters.Add(filterProperties);
        }

        public override IEnumerable<dynamic> Get(GetModel model)
        {
            throw new NotImplementedException();
        }
    }
}