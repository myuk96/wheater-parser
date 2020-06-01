using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CrawlingSystem.Crawler.Weather.Gismeteo
{
    public class GismeteoDateTimeExtractor
    {
        private static readonly Regex DateTimeRegex = new Regex(@"([а-яA-Я]){2},\s(?<day>\d{1,2})\s(?<month>[а-яA-Я]+)", RegexOptions.Compiled);
        private readonly Dictionary<string, int> _monthNameToNumber = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
        {
            {"Января", 1},
            {"Февраля", 2},
            {"Марта", 3},
            {"Апреля", 4},
            {"Мая", 5},
            {"Июня", 6},
            {"Июля", 7},
            {"Августа", 8},
            {"Сентября", 9},
            {"Октября", 10},
            {"Ноября", 11},
            {"Декабря", 12},
        };

        public DateTime Extract(string dateStr)
        {
            var day = Convert.ToInt32(DateTimeRegex.Match(dateStr).Groups["day"].Value);
            var month = Convert.ToInt32(_monthNameToNumber[DateTimeRegex.Match(dateStr).Groups["month"].Value]);

            return new DateTime(DateTime.Now.Year, month, day);
        }
    }
}
