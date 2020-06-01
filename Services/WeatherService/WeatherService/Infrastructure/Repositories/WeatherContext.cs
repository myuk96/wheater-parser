using MongoDB.Driver;
using WeatherService.Models;

namespace WeatherService.Infrastructure.Repositories
{
    public class WeatherContext
    {
        private readonly IMongoDatabase _database;

        public WeatherContext(DbSettings settings)
        {
            _database = new MongoClient(settings.ConnectionString).GetDatabase(settings.Database);
        }

        public IMongoCollection<WeatherInfo> WeatherCollection => _database.GetCollection<WeatherInfo>("weather");
    }
}