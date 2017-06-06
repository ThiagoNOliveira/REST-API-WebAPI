using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http.Formatting;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Dependencies;
using System.Web.Http.Description;
using System.Web.Http.Dispatcher;
using System.Web.Http.ExceptionHandling;
using SDammann.WebApi.Versioning;
using SDammann.WebApi.Versioning.Configuration;
using SDammann.WebApi.Versioning.Discovery;
using SDammann.WebApi.Versioning.Documentation;
using SDammann.WebApi.Versioning.Request;
using TinyIoC;
using WebApiSample.Areas.HelpPage;
using WebApiSample.Infrastructure.Formatters;
using WebApiSample.Infrastructure.Handlers;

namespace WebApiSample
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Cors
            config.EnableCors();

            //Formaters
            config.Formatters.Add(new CsvFormatter(new QueryStringMapping("format", "csv", "text/csv")));
            config.Formatters.Add(new XmlFormatter(new QueryStringMapping("format", "xml", "application/xml")));

            // API Documentation
            config.SetDocumentationProvider(new XmlDocumentationProvider(HostingEnvironment.MapPath("~/bin/" + typeof(WebApiConfig).Assembly.GetName().Name + ".xml")));

            var dependencyContainer = new TinyIoCContainer();

            // API versioning
            config.Services.Replace(typeof(IHttpControllerSelector), new VersionedApiControllerSelector(config));
            config.Services.Replace(typeof(IApiExplorer), new VersionedApiExplorer(config));
            config.DependencyResolver = new DependencyResolver(dependencyContainer);

            dependencyContainer.Register((c, np) => new DefaultControllerIdentificationDetector(config));
            dependencyContainer.Register((c, np) => new DefaultRequestControllerIdentificationDetector(config));

            ApiVersioning.Configure(config).ConfigureRequestVersionDetector<DefaultRouteKeyVersionDetector>();

            // Web API routes
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute("DefaultApi", "api/v{version}/{controller}/{id}", new {id = RouteParameter.Optional});

            //Handlers
            config.Services.Replace(typeof(IExceptionHandler), new GlobalExceptionHandler());
            config.MessageHandlers.Add(new LanguageNegotiationHandler());

            FilterConfig.RegisterHttpFilters(config.Filters);
        }

        private sealed class DependencyResolver : IDependencyResolver
        {
            private readonly TinyIoCContainer _dependencyContainer;

            public DependencyResolver(TinyIoCContainer dependencyContainer)
            {
                _dependencyContainer = dependencyContainer;
            }

            /// <summary>
            ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                // no-op
            }

            /// <summary>
            ///     Retrieves a service from the scope.
            /// </summary>
            /// <returns>
            ///     The retrieved service.
            /// </returns>
            /// <param name="serviceType">The service to be retrieved.</param>
            public object GetService(Type serviceType)
            {
                try
                {
                    return _dependencyContainer.Resolve(serviceType);
                }
                catch (TinyIoCResolutionException ex)
                {
                    Debug.WriteLine("Exception in resolving {0}: {1}", serviceType, ex.Message);
                    return null;
                }
            }

            /// <summary>
            ///     Retrieves a collection of services from the scope.
            /// </summary>
            /// <returns>
            ///     The retrieved collection of services.
            /// </returns>
            /// <param name="serviceType">The collection of services to be retrieved.</param>
            public IEnumerable<object> GetServices(Type serviceType)
            {
                try
                {
                    return _dependencyContainer.ResolveAll(serviceType);
                }
                catch (TinyIoCResolutionException ex)
                {
                    Debug.WriteLine("Exception in resolving {0}: {1}", serviceType, ex.Message);
                    return null;
                }
            }

            /// <summary>
            ///     Starts a resolution scope.
            /// </summary>
            /// <returns>
            ///     The dependency scope.
            /// </returns>
            public IDependencyScope BeginScope()
            {
                // no -op
                return new DependencyResolver(_dependencyContainer);
            }
        }
    }
}