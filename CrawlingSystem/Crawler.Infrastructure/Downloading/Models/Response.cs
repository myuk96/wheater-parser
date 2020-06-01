using System;
using System.Text;

namespace CrawlingSystem.Crawler.Infrastructure.Downloading.Models
{
    public class Response
    {
        public byte[] Content { get; }

        public Encoding Encoding { get; }

        public Uri RequestUri { get; }

        public Response(byte[] content, Encoding encoding, Uri requestUri)
        {
            RequestUri = requestUri ?? throw new ArgumentNullException(nameof(requestUri));
            Content = content ?? throw new ArgumentNullException(nameof(content));
            Encoding = encoding ?? throw new ArgumentNullException(nameof(encoding));
        }
    }
}
