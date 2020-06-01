using System;
using CrawlingSystem.Crawler.Weather.Models;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace CrawlingSystem.Crawler.Weather.Infrastructure.WeatherRepository
{
    internal class WeatherRepository : IWeatherRepository
    {
        private readonly WeatherContext _context;

        public WeatherRepository(DbSettings settings)
        {
            _ = settings ?? throw new ArgumentNullException(nameof(settings));
            _context = new WeatherContext(settings);
        }

        public async Task AddOrUpdateWeatherInfoAsync(WeatherInfo weatherInfo, CancellationToken cancellationToken)
        {
            await _context.WeatherCollection.FindOneAndUpdateAsync(Builders<WeatherInfo>.Filter.And(
                    Builders<WeatherInfo>.Filter.Eq(n => n.ShortDate, weatherInfo.ShortDate),
                    Builders<WeatherInfo>.Filter.Eq(n => n.City, weatherInfo.City)),
                Builders<WeatherInfo>.Update.Combine(
                    Builders<WeatherInfo>.Update.Set(v => v.TemperatureMin, weatherInfo.TemperatureMin),
                    Builders<WeatherInfo>.Update.Set(v => v.TemperatureMax, weatherInfo.TemperatureMax),
                    Builders<WeatherInfo>.Update.SetOnInsert(v => v.City, weatherInfo.City),
                    Builders<WeatherInfo>.Update.SetOnInsert(v => v.ShortDate, weatherInfo.ShortDate)),
                new FindOneAndUpdateOptions<WeatherInfo>()
                {
                    IsUpsert = true
                }, cancellationToken).ConfigureAwait(false);
        }
    }
}
