using CrawlingSystem.Crawler.Infrastructure.Downloading;
using CrawlingSystem.Crawler.Weather.Infrastructure.WeatherRepository;
using CrawlingSystem.Crawler.Weather.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using log4net;
using log4net.Config;

namespace CrawlingSystem.Crawler.Weather
{
    public abstract class WeatherCrawlerPipelineBuilderBase
    {
        protected IServiceProvider ServiceProvider { get; private set; }
        private readonly IServiceCollection _serviceCollection;

        protected WeatherCrawlerPipelineBuilderBase(WeatherCrawlerSettings settings)
        { 
            _serviceCollection = new ServiceCollection()
                .AddSingleton<IDownloadService, HttpDownloadService>()
                .AddSingleton<ILog>(sp =>
                {
                    var log = LogManager.GetLogger(GetType());
                    XmlConfigurator.Configure(log.Logger.Repository, new FileInfo("log4net.config"));
                    return log;
                })
                .AddSingleton<IWeatherRepository, WeatherRepository>()
                .AddTransient<DbSettings>(sp => new DbSettings()
                {
                    ConnectionString = settings.ConnectionString,
                    Database = settings.Database
                });
        }

        public Func<Uri, Task> BuildPipeline()
        {
            Register(_serviceCollection);
            ServiceProvider = _serviceCollection.BuildServiceProvider();
            return Build(ServiceProvider);
        }

        protected abstract void Register(IServiceCollection serviceCollection);
        protected abstract Func<Uri, Task> Build(IServiceProvider serviceProvider);
    }
}
