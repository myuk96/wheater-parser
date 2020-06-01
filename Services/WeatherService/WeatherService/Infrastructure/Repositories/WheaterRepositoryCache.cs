using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Caching.Memory;
using WeatherService.Models;

namespace WeatherService.Infrastructure.Repositories
{
    public class WeatherRepositoryCache : IWeatherRepository
    {
        private readonly MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
        private readonly IWeatherRepository _innerRepository;

        public WeatherRepositoryCache(IWeatherRepository innerRepository)
        {
            _innerRepository = innerRepository ?? throw new ArgumentNullException(nameof(innerRepository));
        }

        public async Task<WeatherInfo> GetWeatherAsync(GetWeatherInfoRequest request)
        {
            var key = ("GetWeatherAsync_" + request.ShortDate + request.City).GetHashCode();

            if (_cache.TryGetValue(key, out var result)) return await Task.FromResult((WeatherInfo)result);

            var originalResponse = await _innerRepository.GetWeatherAsync(request).ConfigureAwait(false);

            _cache.Set(key, originalResponse, TimeSpan.FromMinutes(5));

            return originalResponse;
        }

        public async Task<List<string>> GetCityListAsync()
        {
            var key = "GetCityListAsync_".GetHashCode();

            if (_cache.TryGetValue(key, out var result)) return await Task.FromResult((List<string>)result);

            var originalResponse = await _innerRepository.GetCityListAsync().ConfigureAwait(false);

            _cache.Set(key, originalResponse, TimeSpan.FromMinutes(5));

            return originalResponse;
        }
    }
}