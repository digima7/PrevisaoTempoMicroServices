using MicroServiceBase.Communication;
using MicroServiceBase.Model;

namespace MicroService2
{
    class WeatherConsumer : IMessageConsumer<WeatherRequest>
    {
        private readonly IWeatherService _service;
        private readonly IMessage _messaging;

        public WeatherConsumer(IWeatherService service, IMessage messaging)
        {
            _service = service;
            _messaging = messaging;
        }

        public void Receive(WeatherRequest message)
        {
            var result = _service.QueryForecast(message);
            _messaging.Publish(result);
        }
    }
}