using System;

namespace CrawlingSystem.Crawler.Weather.Infrastructure.Extractors
{
    public interface IEntityExtractor<T>
    {
        T Extract(Func<string> htmlFactory);
    }
}