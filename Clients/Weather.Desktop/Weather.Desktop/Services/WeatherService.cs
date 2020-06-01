using ServiceReference1;
using System;
using System.Threading.Tasks;

namespace Weather.Desktop
{
    internal class WeatherService
    {
        private readonly WeatherServiceClient _weatherServiceClient;

        public WeatherService()
        {
            _weatherServiceClient = new WeatherServiceClient();
        }

        public async Task<string[]> GetAllCitiesAsync()
        {
            return await _weatherServiceClient.GetCitiesAsync().ConfigureAwait(false);
        }

        public async Task<WeatherInfo> GetTomorrowWeatherAsync(string city)
        {
            if (string.IsNullOrEmpty(city)) throw new ArgumentException(nameof(city));

            var response = await _weatherServiceClient.GetWeatherAsync(new GetWeatherInfoRequest()
            {
                City = city,
                ShortDate = DateTime.Now.AddDays(1).ToShortDateString()
            }).ConfigureAwait(false);

            return new WeatherInfo()
            {
                City = response.City,
                Date = response.ShortDate,
                MaxTemperature = response.TemperatureMax,
                MinTemperature = response.TemperatureMin
            };
        }
    }
}
