using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WebApiSample.Infrastructure.Handlers
{
    public class CompressionNegotiationHandler : DelegatingHandler
    {
        public CompressionNegotiationHandler()
        {
            Compressors = new Collection<ICompressor> {new GZipCompressor(), new DeflateCompressor()};
        }

        public Collection<ICompressor> Compressors { get; }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);

            if (request.Headers.AcceptEncoding.Any() && response.Content != null)
            {
                ICompressor compressor;


                if (request.Headers.AcceptEncoding.Any(x => x.Value == "deflate"))
                    compressor = Compressors.FirstOrDefault(x => x.EncodingType == "deflate");
                else
                    compressor = (from encoding in request.Headers.AcceptEncoding
                        let quality = encoding.Quality ?? 1.0
                        where quality > 0
                        join c in Compressors on encoding.Value.ToLowerInvariant() equals
                        c.EncodingType.ToLowerInvariant()
                        orderby quality descending
                        select c).FirstOrDefault();

                if (compressor != null)
                    response.Content = new CompressedContent(response.Content, compressor);
            }

            return response;
        }
    }

    public interface ICompressor
    {
        string EncodingType { get; }
        Task Compress(Stream source, Stream destination);
        Task Decompress(Stream source, Stream destination);
    }

    public class GZipCompressor : Compressor
    {
        private const string GZipEncoding = "gzip";

        public override string EncodingType => GZipEncoding;

        public override Stream CreateCompressionStream(Stream output)
        {
            return new GZipStream(output, CompressionMode.Compress, true);
        }

        public override Stream CreateDecompressionStream(Stream input)
        {
            return new GZipStream(input, CompressionMode.Decompress, true);
        }
    }

    public class DeflateCompressor : Compressor
    {
        private const string DeflateEncoding = "deflate";

        public override string EncodingType => DeflateEncoding;

        public override Stream CreateCompressionStream(Stream output)
        {
            return new DeflateStream(output, CompressionMode.Compress, true);
        }

        public override Stream CreateDecompressionStream(Stream input)
        {
            return new DeflateStream(input, CompressionMode.Decompress, true);
        }
    }

    public abstract class Compressor : ICompressor
    {
        public abstract string EncodingType { get; }

        public virtual Task Compress(Stream source, Stream destination)
        {
            var compressed = CreateCompressionStream(destination);

            return Pump(source, compressed)
                .ContinueWith(task => compressed.Dispose());
        }

        public virtual Task Decompress(Stream source, Stream destination)
        {
            var decompressed = CreateDecompressionStream(source);

            return Pump(decompressed, destination)
                .ContinueWith(task => decompressed.Dispose());
        }

        public abstract Stream CreateCompressionStream(Stream output);
        public abstract Stream CreateDecompressionStream(Stream input);

        protected virtual Task Pump(Stream input, Stream output)
        {
            return input.CopyToAsync(output);
        }
    }

    public class CompressedContent : HttpContent
    {
        private readonly ICompressor _compressor;
        private readonly HttpContent _content;

        public CompressedContent(HttpContent content, ICompressor compressor)
        {
            _content = content;
            _compressor = compressor;

            AddHeaders();
        }

        protected override bool TryComputeLength(out long length)
        {
            length = -1;
            return false;
        }

        protected override async Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            using (_content)
            {
                var contentStream = await _content.ReadAsStreamAsync();
                await _compressor.Compress(contentStream, stream);
            }
        }

        private void AddHeaders()
        {
            foreach (var header in _content.Headers)
                Headers.TryAddWithoutValidation(header.Key, header.Value);

            Headers.ContentEncoding.Add(_compressor.EncodingType);
        }
    }
}