using System.Web.Http.Filters;
using System.Web.Mvc;
using WebApiSample.Infrastructure.Filters;

namespace WebApiSample
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        internal static void RegisterHttpFilters(HttpFilterCollection filters)
        {
            filters.Add(new ModelStateValidatorFilter());
        }
    }
}