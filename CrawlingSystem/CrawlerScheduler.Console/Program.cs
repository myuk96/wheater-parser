using CrawlingSystem.Crawler.Weather.Gismeteo;
using CrawlingSystem.Crawler.Weather.Models;
using FluentScheduler;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace CrawlerScheduler.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var registry = new Registry();

            registry.Schedule(async () =>
                {
                    await GetGismeteoPipelineEntry()(new Uri("https://www.gismeteo.ru/"));
                })
                .ToRunNow()
                .AndEvery(30)
                .Seconds();

            JobManager.Initialize(registry);

            System.Console.ReadKey();
        }

        private static Func<Uri, Task> GetGismeteoPipelineEntry()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            return new GismeteoCrawlerPipelineBuilder(
                new WeatherCrawlerSettings()
                {
                    Database = configuration["Database"],
                    ConnectionString = configuration["ConnectionString"]
                }).BuildPipeline();
        }
    }
}
