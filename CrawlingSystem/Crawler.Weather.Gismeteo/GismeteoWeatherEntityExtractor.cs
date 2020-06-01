using CrawlingSystem.Crawler.Weather.Infrastructure.Extractors;
using CrawlingSystem.Crawler.Weather.Models;
using HtmlAgilityPack;
using System;

namespace CrawlingSystem.Crawler.Weather.Gismeteo
{
    internal class GismeteoEntityExtractor : IEntityExtractor<WeatherInfo>
    {
        private readonly GismeteoWeatherEntityXPathInfo _entityXPathInfo;
        private readonly GismeteoDateTimeExtractor _dateTimeExtractor;

        public GismeteoEntityExtractor(GismeteoWeatherEntityXPathInfo entityXPathInfo, GismeteoDateTimeExtractor dateTimeExtractor)
        {
            _entityXPathInfo = entityXPathInfo ?? throw new ArgumentNullException(nameof(entityXPathInfo));
            _dateTimeExtractor = dateTimeExtractor ?? throw new ArgumentNullException(nameof(dateTimeExtractor)); ;
        }

        public WeatherInfo Extract(Func<string> htmlFactory)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlFactory());

            var result = new WeatherInfo
            {
                City = doc.DocumentNode.SelectSingleNode(_entityXPathInfo.City).InnerText,
                ShortDate = _dateTimeExtractor.Extract(doc.DocumentNode.SelectSingleNode(_entityXPathInfo.Date).InnerText).ToShortDateString(),
                TemperatureMax = doc.DocumentNode.SelectSingleNode(_entityXPathInfo.TemperatureMax).InnerText,
                TemperatureMin = doc.DocumentNode.SelectSingleNode(_entityXPathInfo.TemperatureMin).InnerText,
            };

            return result;
        }
    }
}
