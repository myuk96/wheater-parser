using System;
using System.Threading;
using System.Threading.Tasks;
using CrawlingSystem.Crawler.Weather.Infrastructure.WeatherRepository;
using CrawlingSystem.Crawler.Weather.Models;
using log4net;
using MongoDB.Bson;

namespace CrawlingSystem.Crawler.Weather.Pipeline
{
    public class EntitySaver
    {
        private readonly IWeatherRepository _weatherRepository;
        private readonly ILog _logger;

        public EntitySaver(IWeatherRepository weatherRepository, ILog logger)
        {
            _weatherRepository = weatherRepository ?? throw new ArgumentNullException(nameof(weatherRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task AddAsync(WeatherInfo input)
        {
            _logger.Info($"Weather for {input.City} adds to storage");

            try
            {
                await _weatherRepository.AddOrUpdateWeatherInfoAsync(input, CancellationToken.None);
            }
            catch (Exception e)
            {
                _logger.Error("Error when adding entity to repository", e);
                throw;
            }
        }
    }
}
