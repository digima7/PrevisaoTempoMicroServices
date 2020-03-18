using Nancy;
using Nancy.ModelBinding;
using Nancy.TinyIoc;

namespace MicroService1
{
    public class WeatherModule : NancyModule
    {
        public WeatherModule(IWeatherController controller)
        {
            Get("/", _ => controller.GetAllCities());
            Get("/{city}", p => controller.GetCity(p.city));
            Post("/", p =>
            {
                var city = this.Bind<City>();
                return controller.AddCity(city);
            });
        }
    }
    
    public class WeatherBootstrap : DefaultNancyBootstrapper
    {
        private readonly IWeatherController _controller;

        public WeatherBootstrap(IWeatherController controller)
        {
            _controller = controller;
        }

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            container.Register(_controller);
        }
    }
}