using System;
using System.Web;

namespace WebApiSample.Infrastructure.HttpModules
{
    public class UniqueRequestIdentifierModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.BeginRequest += AddRequestIdOnHttpContextItems;
        }

        public void Dispose()
        {
        }

        private static void AddRequestIdOnHttpContextItems(object sender, EventArgs e)
        {
            if (HttpContext.Current == null) return;

            var requestId = HttpContext.Current.Request.Headers["x-RequestId"];

            if (string.IsNullOrWhiteSpace(requestId))
                requestId = Guid.NewGuid().ToString();

            HttpContext.Current.Items.Add("RequestId", requestId);
        }
    }
}