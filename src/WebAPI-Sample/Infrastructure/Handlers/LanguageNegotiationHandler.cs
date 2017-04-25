using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace WebApiSample.Infrastructure.Handlers
{
    public class LanguageNegotiationHandler : DelegatingHandler
    {
        #region Fields

        private const string PtBr = "pt-BR";
        private const string EnUs = "en-US";
        private const string EsCo = "es-CO";
        private readonly List<string> _supportedLanguages = new List<string> {PtBr, EnUs, EsCo};

        #endregion

        #region Methods

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            if (!SetHeaderIfAcceptLanguageMatchesSupportedLanguage(request))
                if (!SetHeaderIfGlobalAcceptLanguageMatchesSupportedLanguage(request))
                    SetCulture(request, PtBr);

            var response = await base.SendAsync(request, cancellationToken);
            return response;
        }

        private bool SetHeaderIfAcceptLanguageMatchesSupportedLanguage(HttpRequestMessage request)
        {
            foreach (var lang in request.Headers.AcceptLanguage.Where(lang => _supportedLanguages.Contains(lang.Value)))
            {
                SetCulture(request, lang.Value);

                return true;
            }

            return false;
        }

        private bool SetHeaderIfGlobalAcceptLanguageMatchesSupportedLanguage(HttpRequestMessage request)
        {
            foreach (var globalLang in request.Headers.AcceptLanguage.Select(lang => lang.Value.Substring(0, 2))
                .Where(globalLang => _supportedLanguages.Any(t => t.StartsWith(globalLang))))
            {
                SetCulture(request, _supportedLanguages.FirstOrDefault(i => i.StartsWith(globalLang)));

                return true;
            }

            return false;
        }

        private static void SetCulture(HttpRequestMessage request, string lang)
        {
            request.Headers.AcceptLanguage.Clear();
            request.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue(lang));

            Thread.CurrentThread.CurrentCulture = new CultureInfo(lang);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.CreateSpecificCulture(lang);
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.CreateSpecificCulture(lang);
        }

        #endregion
    }
}