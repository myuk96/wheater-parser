using System;
using System.Linq;
using HtmlAgilityPack;

namespace CrawlingSystem.Crawler.Infrastructure.Extractors
{
    public class HtmlUrlExtractor
    {
        private readonly Uri _baseUri;

        public HtmlUrlExtractor(Uri baseUri)
        {
            _baseUri = baseUri;
        }

        public Uri[] Extract(Func<string> htmlFactory, string xPath)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlFactory());

            return doc.DocumentNode.SelectNodes(xPath)?
                .Where(v => v.GetAttributeValue("href", null) != null)
                .Select(v => new Uri(_baseUri, v.GetAttributeValue("href", null))).ToArray() ?? Array.Empty<Uri>();
        }
    }
}
