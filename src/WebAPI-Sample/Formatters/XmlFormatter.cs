using System.Net.Http.Formatting;

namespace WebApiSample.Formatters
{
    public class XmlFormatter : XmlMediaTypeFormatter
    {
        public XmlFormatter(MediaTypeMapping mediaTypeMapping)
        {
            MediaTypeMappings.Add(mediaTypeMapping);
        }
    }
}