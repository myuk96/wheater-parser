using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CrawlingSystem.Crawler.Weather.Models
{
    public class WeatherInfo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string City { get; set; }

        public string ShortDate { get; set; }

        public string TemperatureMin { get; set; }

        public string TemperatureMax { get; set; }
    }
}
