using CrawlingSystem.Crawler.Weather.Models;
using System.Threading;
using System.Threading.Tasks;

namespace CrawlingSystem.Crawler.Weather.Infrastructure.WeatherRepository
{
    public interface IWeatherRepository
    {
        Task AddOrUpdateWeatherInfoAsync(WeatherInfo weatherInfo, CancellationToken cancellationToken);
    }
}
