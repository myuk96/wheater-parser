using CrawlingSystem.Crawler.Weather.Infrastructure.Extractors;
using CrawlingSystem.Crawler.Weather.Models;
using System;
using System.Text;
using CrawlingSystem.Crawler.Infrastructure.Downloading.Models;
using log4net;

namespace CrawlingSystem.Crawler.Weather.Pipeline
{
    public class EntityExtractor
    {
        private readonly IEntityExtractor<WeatherInfo> _infoExtractor;
        private readonly ILog _logger;

        public EntityExtractor(IEntityExtractor<WeatherInfo> weatherEntityExtractor, ILog logger)
        {
            _infoExtractor = weatherEntityExtractor ?? throw new ArgumentNullException(nameof(weatherEntityExtractor));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public WeatherInfo Extract(Response response)
        {
            _logger.Info($"Extracting entity for {response.RequestUri} response");

            try
            {
                return _infoExtractor.Extract(() => Encoding.UTF8.GetString(response.Content));
            }
            catch (Exception e)
            {
                _logger.Error("Error when extracting entity", e);
                throw;
            }
        }
    }
}
