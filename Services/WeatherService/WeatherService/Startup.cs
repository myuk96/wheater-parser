using System;
using System.IO;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using log4net;
using log4net.Config;
using WeatherService.Infrastructure.Repositories;
using WeatherService.Models;

namespace WeatherService.Infrastructure
{
    public class Startup : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(

                Component.For<WeatherRepository>().LifestyleSingleton(),
                Component.For<IWeatherRepository>().LifestyleSingleton()
                    .UsingFactoryMethod(v => new WeatherRepositoryCache(v.Resolve<WeatherRepository>())),
                Component.For<DbSettings>().Instance(new DbSettings()
                    {
                        ConnectionString =
                            "mongodb://127.0.0.1:27017/?readPreference=primary&appname=MongoDB%20Compass%20Community&ssl=false",
                        Database = "weather_db"
                    })
                    .LifestyleSingleton(),
                Component.For<ILog>().LifestyleSingleton().UsingFactoryMethod(() =>
                {
                    var log = LogManager.GetLogger(GetType());
                    XmlConfigurator.Configure(log.Logger.Repository, new FileInfo("log4net.config"));
                    return log;
                }));
        }
    }
}