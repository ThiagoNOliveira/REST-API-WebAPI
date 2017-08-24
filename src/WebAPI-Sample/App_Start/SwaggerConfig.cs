using System.Web.Http;
using WebActivatorEx;
using WebApiSample;
using Swashbuckle.Application;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace WebApiSample
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                {
                    c.Schemes(new[] { "http" });
                    c.SingleApiVersion("v1", "WebApi - Sample");
                    c.PrettyPrint();
                    c.IncludeXmlComments(GetXmlCommentsPath());
                    c.DescribeAllEnumsAsStrings();
                })
                .EnableSwaggerUi(c =>
                {
                    c.DocumentTitle("Solutions One - Billing API");
                });
        }

        protected static string GetXmlCommentsPath()
        {
            return System.String.Format(@"{0}\App_Data\XmlDocument.xml", System.AppDomain.CurrentDomain.BaseDirectory);
        }
    }
}
