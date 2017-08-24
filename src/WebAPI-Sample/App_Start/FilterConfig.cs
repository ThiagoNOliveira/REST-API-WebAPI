using System.Web.Http.Filters;
using WebApiSample.Infrastructure.Filters;

namespace WebApiSample
{
    public static class FilterConfig
    {
        internal static void RegisterHttpFilters(HttpFilterCollection filters)
        {
            filters.Add(new ModelStateValidatorFilter());
        }
    }
}