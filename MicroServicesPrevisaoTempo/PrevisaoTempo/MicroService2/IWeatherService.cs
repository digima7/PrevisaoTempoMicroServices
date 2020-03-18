using MicroServiceBase.Model;

namespace MicroService2
{
    internal interface IWeatherService
    {
        WeatherReport QueryForecast(WeatherRequest request);
    }
}