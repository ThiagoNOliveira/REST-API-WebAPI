using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;

namespace WebApiSample.Infrastructure.Formatters
{
    public class CsvFormatter : BufferedMediaTypeFormatter
    {
        private static readonly char[] _specialChars = {',', '\n', '\r', '"'};

        public CsvFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/csv"));
            SupportedEncodings.Add(new UTF8Encoding(false));
            SupportedEncodings.Add(Encoding.GetEncoding("iso-8859-1"));
        }

        public CsvFormatter(MediaTypeMapping mediaTypeMapping)
            : this()
        {
            MediaTypeMappings.Add(mediaTypeMapping);
        }

        public override bool CanWriteType(Type type)
        {
            if (type == typeof(object))
                return true;

            var enumerableType = typeof(IEnumerable<object>);
            return enumerableType.IsAssignableFrom(type);
        }

        public override bool CanReadType(Type type)
        {
            return false;
        }

        public override void WriteToStream(Type type, object value, Stream writeStream, HttpContent content)
        {
            var effectiveEncoding = SelectCharacterEncoding(content.Headers);

            using (var writer = new StreamWriter(writeStream, effectiveEncoding))
            {
                var products = value as IEnumerable<object>;
                if (products != null)
                {
                    foreach (var product in products)
                        WriteItem(product, writer);
                }
                else
                {
                    var singleProduct = value;
                    if (singleProduct == null)
                        throw new InvalidOperationException("Cannot serialize type");
                    WriteItem(singleProduct, writer);
                }
            }
        }

        // Helper methods for serializing Products to CSV format. 
        private static void WriteItem(dynamic product, StreamWriter writer)
        {
            writer.WriteLine("{0},{1},{2},{3}", Escape(product.Id),
                Escape(product.Name), Escape(product.Category), Escape(product.Price));
        }

        private static string Escape(object o)
        {
            if (o == null) return "";

            var field = o.ToString();
            return field.IndexOfAny(_specialChars) != -1 ? string.Format("\"{0}\"", field.Replace("\"", "\"\"")) : field;
        }
    }
}