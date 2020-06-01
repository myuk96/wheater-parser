using CrawlingSystem.Crawler.Infrastructure.Downloading;
using CrawlingSystem.Crawler.Infrastructure.Downloading.Models;
using CrawlingSystem.Crawler.Infrastructure.Extractors;
using CrawlingSystem.Crawler.Weather.Extensions;
using CrawlingSystem.Crawler.Weather.Infrastructure.Extractors;
using CrawlingSystem.Crawler.Weather.Models;
using CrawlingSystem.Crawler.Weather.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using log4net;

namespace CrawlingSystem.Crawler.Weather.Gismeteo
{
    public class GismeteoCrawlerPipelineBuilder : WeatherCrawlerPipelineBuilderBase
    {
        public GismeteoCrawlerPipelineBuilder(WeatherCrawlerSettings settings)
        : base(settings)
        {
        }

        protected override void Register(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<NestedUrlExtractorSettings>(sp => new NestedUrlExtractorSettings()
                {
                    NestedUrlXPath = "//a[contains(@class, 'cities_link')] | //noscript[contains(@id, 'noscript')]/a"
                })
                .AddSingleton<HtmlUrlExtractor>(sp => new HtmlUrlExtractor(new Uri("https://www.gismeteo.ru/")))
                .AddSingleton<GismeteoDateTimeExtractor>()
                .AddTransient<GismeteoWeatherEntityXPathInfo>(sp => new GismeteoWeatherEntityXPathInfo()
                {
                    City = "/html/body/section/nav/div/div/div[1]",
                    Date = "/html/body/section/div[2]/div/div[1]/div/div[2]/div[1]/div[1]/a[2]/div/div[1]/div[1]/span",
                    TemperatureMax =
                        "/html/body/section/div[2]/div/div[1]/div/div[2]/div[1]/div[1]/a[2]/div/div[1]/div[3]/div/div/div/div[2]/span[1]",
                    TemperatureMin =
                        "/html/body/section/div[2]/div/div[1]/div/div[2]/div[1]/div[1]/a[2]/div/div[1]/div[3]/div/div/div/div[1]/span[1]"
                })
                .AddSingleton<IEntityExtractor<WeatherInfo>, GismeteoEntityExtractor>()
                .AddSingleton<NestedUrlExtractor>()
                .AddSingleton<EntitySaver>()
                .AddSingleton<EntityExtractor>();
        }

        protected override Func<Uri, Task> Build(IServiceProvider serviceProvider)
        {
            //pub-sub pattern 
            var pipelineEntry = new BufferBlock<Uri>();

            var resultTask = pipelineEntry
                .Select(async v =>
                    await serviceProvider.GetService<IDownloadService>().DownloadAsync(new Request() { Uri = v }))
                .SelectMany(v => serviceProvider.GetService<NestedUrlExtractor>().Extract(v))
                .Select(async v =>
                    await serviceProvider.GetService<IDownloadService>().DownloadAsync(new Request() { Uri = v }))
                .Select(v => serviceProvider.GetService<EntityExtractor>().Extract(v))
                .ForEach(async v => await serviceProvider.GetService<EntitySaver>().AddAsync(v));

            return uri =>
            {
                serviceProvider.GetService<ILog>().Info($"Crawling has started for {uri.OriginalString}");

                pipelineEntry.Post(uri);
                pipelineEntry.Complete();

                return resultTask.Completion;
            };
        }
    }
}
