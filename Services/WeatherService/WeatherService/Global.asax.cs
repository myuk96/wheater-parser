using Castle.Facilities.WcfIntegration;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using System;

namespace WeatherService
{
    public class Global : System.Web.HttpApplication
    {
        private static readonly IWindsorContainer ContainerInstance = new WindsorContainer();

        protected void Application_Start(object sender, EventArgs e)
        {
            ContainerInstance.AddFacility<WcfFacility>();
            ContainerInstance.Register(Component.For<IWeatherService, WeatherService>());
            ContainerInstance.Install(FromAssembly.This());
        }
    }
}