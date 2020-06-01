using System;
using System.Runtime.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WeatherService.Models
{
    [DataContract]
    public class WeatherInfo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [IgnoreDataMember]
        public string Id { get; set; }

        [DataMember]
        public string City { get; set; }

        [DataMember]
        public string ShortDate { get; set; }

        [DataMember]
        public string TemperatureMin { get; set; }

        [DataMember]
        public string TemperatureMax { get; set; }

        [DataMember]
        public string SourceName { get; set; }
    }
}