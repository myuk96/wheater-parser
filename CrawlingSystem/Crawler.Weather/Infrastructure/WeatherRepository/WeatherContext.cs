using CrawlingSystem.Crawler.Weather.Models;
using MongoDB.Driver;

namespace CrawlingSystem.Crawler.Weather.Infrastructure.WeatherRepository
{
    internal class WeatherContext
    {
        private readonly IMongoDatabase _database;

        public WeatherContext(DbSettings settings)
        {
            _database = new MongoClient(settings.ConnectionString).GetDatabase(settings.Database);
        }

        public IMongoCollection<WeatherInfo> WeatherCollection => _database.GetCollection<WeatherInfo>("weather");
    }
}
