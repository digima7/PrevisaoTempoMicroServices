namespace MicroService1
{
    internal interface IWeatherUpdateService
    {
        void Start();
        void Append(CityWeather city);
    }
}