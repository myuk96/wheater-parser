using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherService.Models;

namespace WeatherService.Infrastructure.Repositories
{
    public interface IWeatherRepository
    {
        Task<WeatherInfo> GetWeatherAsync(GetWeatherInfoRequest request);
        Task<List<string>> GetCityListAsync();
    }
}
