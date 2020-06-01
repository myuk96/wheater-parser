using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherService.Models;

namespace WeatherService.Infrastructure.Repositories
{
    public class WeatherRepository : IWeatherRepository
    {
        private readonly WeatherContext _context;

        public WeatherRepository(DbSettings settings)
        {
            _ = settings ?? throw new ArgumentNullException(nameof(settings));
            _context = new WeatherContext(settings);
        }

        public async Task<List<string>> GetCityListAsync()
        {
            var groupingResult = await _context.WeatherCollection
                    .Aggregate()
                    .Group(v => v.City, group => new {Result = group.Key})
                    .ToListAsync()
                    .ConfigureAwait(false);

                return groupingResult.Select(v => v.Result).ToList();
        }

        public async Task<WeatherInfo> GetWeatherAsync(GetWeatherInfoRequest request)
        {
            return await _context.WeatherCollection.Find(Builders<WeatherInfo>.Filter.And(

                Builders<WeatherInfo>.Filter.Eq(v => v.ShortDate, request.ShortDate),
                Builders<WeatherInfo>.Filter.Eq(v => v.City, request.City)
            )).SingleAsync().ConfigureAwait(false);
        }
    }
}