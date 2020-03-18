using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroServiceBase.Communication;
using MicroServiceBase.Model;

namespace MicroService1
{
    class WeatherUpdateService : IMessageConsumer<WeatherReport>, IWeatherUpdateService
    {
        private readonly TimeSpan _waitTime;
        private readonly WeatherDatabase _database;
        private readonly IMessage _message;
        private Task _task;
        private List<CityWeather> _cities;

        public WeatherUpdateService(TimeSpan waitTime, WeatherDatabase database, IMessage message)
        {
            _waitTime = waitTime;
            _database = database;
            _message = message;
        }

        public void Start()
        {
            using (var ctx = _database.Context())
                _cities = ctx.CityWeathers.ToList();                
            
            _task = new Task(async () =>
            { 
                while (true)
                {
                    foreach (var city in _cities)
                    {
                        _message.Publish(new WeatherRequest
                        {
                            City = city.Name
                        });
                    }

                    await Task.Delay(_waitTime);
                }
            });
            _task.Start();
        }

        public void Append(CityWeather city)
        {
            _cities?.Add(city);
            _message.Publish(new WeatherRequest
            {
                City = city.Name
            });
        }

        public void Receive(WeatherReport message)
        {
            using (var ctx = _database.Context())
            {
                var city = ctx.CityWeathers.FirstOrDefault(c => c.Name == message.City);
                if (city != null)
                {
                    city.Date = message.Date;
                    city.JSON = message.JSON;
                }

                ctx.SaveChanges();
            }
        }
    }
}