using System;
using MicroServiceBase.Model;

namespace MicroService2
{
    class OpenWeatherService : IWeatherService
    {
        private const string URL =
            "http://api.openweathermap.org/data/2.5/forecast/daily?q={0}&units=metric&cnt=1&APPID={1}";

        private readonly IRequester _requester;
        private readonly string _apiKey;

        public OpenWeatherService(IRequester requester, string apiKey)
        {
            _requester = requester;
            _apiKey = apiKey;
        }

        public WeatherReport QueryForecast(WeatherRequest request)
        {
            var url = string.Format(URL, request.City, _apiKey);
            var json = _requester.Request(url);
            return new WeatherReport
            {
                City = request.City,
                JSON = json,
                Date = DateTime.Now
            };
        }
    }
}