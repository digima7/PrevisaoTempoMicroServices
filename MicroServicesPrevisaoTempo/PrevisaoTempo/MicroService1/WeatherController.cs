using System;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Nancy;

namespace MicroService1
{
    class WeatherController : IWeatherController
    {
        private readonly WeatherDatabase _database;
        private readonly IWeatherUpdateService _updateService;

        public WeatherController(WeatherDatabase database, IWeatherUpdateService updateService)
        {
            _database = database;
            _updateService = updateService;
        }
        
        public Response GetAllCities()
        {
            using (var ctx = _database.Context())
                return RawJSON($"[{string.Join(", ", ctx.CityWeathers.ToList().Select(d => d.JSON))}]");
        }

        public Response GetCity(string cityName)
        {
            using (var ctx = _database.Context())
            {
                var city = ctx.CityWeathers.FirstOrDefault(c => c.Name == cityName.ToUpper());

                if (city != null)
                    return RawJSON(city.JSON);
                return "{}";
            }
        }

        public Response AddCity(City city)
        {
            using (var ctx = _database.Context())
            {
                try
                {
                    var cityWeather = new CityWeather
                    {
                        Name = city.Name.ToUpper(),
                        JSON = "",
                        Date = DateTime.MinValue,
                    };
                    ctx.CityWeathers.Add(cityWeather);
                    ctx.SaveChanges();

                    _updateService.Append(cityWeather);
                    return RawJSON("{}");
                }
                catch (DbUpdateException)
                {
                    return RawJSON("{\"error\": \"City already exists\"}", HttpStatusCode.Conflict);
                }
            }
        }

        private Response RawJSON(string text, HttpStatusCode status=HttpStatusCode.OK)
        {
            var jsonBytes = Encoding.UTF8.GetBytes(text);
            return new Response
            {
                StatusCode = status,
                ContentType = "application/json",
                Contents = s => s.Write(jsonBytes, 0, jsonBytes.Length)
            };
        }
    }
}