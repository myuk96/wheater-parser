using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using log4net;
using WeatherService.Infrastructure.Repositories;
using WeatherService.Models;

namespace WeatherService
{
    public class WeatherService : IWeatherService
    {
        private readonly IWeatherRepository _weatherRepository;
        private readonly ILog _log;

        public WeatherService(IWeatherRepository weatherRepository, ILog log)
        {
            _weatherRepository = weatherRepository ?? throw new ArgumentNullException(nameof(weatherRepository));
            _log = log ?? throw new ArgumentNullException(nameof(log));
        }

        public async Task<List<string>> GetCities()
        {
            try
            {
                _log.Info("Get cities");

                return await _weatherRepository.GetCityListAsync();
            }
            catch (Exception e)
            {
                _log.Error("Error when getting cities", e);
                throw;
            }
        }

        public async Task<WeatherInfo> GetWeather(GetWeatherInfoRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (string.IsNullOrEmpty(request.City)) throw new ArgumentException(nameof(request.City));

            try
            {
                _log.Info($"Get weather for {request.City} on {request.ShortDate}");

                return await _weatherRepository.GetWeatherAsync(request);
            }
            catch (Exception e)
            {
                _log.Error("Error when getting weather", e);
                throw;
            }
        }
    }
}
