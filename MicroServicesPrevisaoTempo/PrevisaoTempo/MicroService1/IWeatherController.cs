using Nancy;

namespace MicroService1
{
    public interface IWeatherController
    {
        Response GetAllCities();
        Response GetCity(string city);
        Response AddCity(City city);
    }
}