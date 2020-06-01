using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Threading.Tasks;
using WeatherService.Models;

namespace WeatherService
{
    [ServiceContract]
    public interface IWeatherService
    {
        [OperationContract]
        Task<List<string>> GetCities();

        [OperationContract]
        Task<WeatherInfo> GetWeather(GetWeatherInfoRequest request);
    }
}
