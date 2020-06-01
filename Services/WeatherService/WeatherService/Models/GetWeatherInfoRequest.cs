using System.Runtime.Serialization;

namespace WeatherService.Models
{
    [DataContract]
    public class GetWeatherInfoRequest
    {

        [DataMember]
        public string City { get; set; }


        [DataMember]
        public string ShortDate { get; set; }
    }
}