using CrawlingSystem.Crawler.Weather.Models;
using System;
using CrawlingSystem.Crawler.Infrastructure.Downloading.Models;
using CrawlingSystem.Crawler.Infrastructure.Extractors;
using log4net;

namespace CrawlingSystem.Crawler.Weather.Pipeline
{
    public class NestedUrlExtractor
    {
        private readonly NestedUrlExtractorSettings _configuration;
        private readonly HtmlUrlExtractor _htmlUrlExtractor;
        private readonly ILog _logger;

        public NestedUrlExtractor(NestedUrlExtractorSettings configuration, HtmlUrlExtractor htmlUrlExtractor, ILog logger)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _htmlUrlExtractor = htmlUrlExtractor ?? throw new ArgumentNullException(nameof(htmlUrlExtractor));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Uri[] Extract(Response response)
        {
            try
            {
                Uri[] nestedUrl = _htmlUrlExtractor.Extract(() => response.Encoding.GetString(response.Content),
                    xPath: _configuration.NestedUrlXPath);

                _logger.Info($"From {response.RequestUri.AbsoluteUri} has extracted {nestedUrl.Length} nested urls");

                return nestedUrl;
            }
            catch (Exception e)
            {
                _logger.Error("Error when extracting nested urls", e);
                throw;
            }
        }
    }
}
