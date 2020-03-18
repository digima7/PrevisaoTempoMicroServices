using System;
using MicroServiceBase.Communication;

namespace MicroService2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Configurando Mensageria");
            var messagingConfig = new MessagingConfiguration 
            { 
                Host = Environment.GetEnvironmentVariable("MESSAGING_HOST") ?? "localhost",
                Port = Convert.ToInt32(Environment.GetEnvironmentVariable("MESSAGING_PORT") ?? "5672"),
                UserName = Environment.GetEnvironmentVariable("MESSAGING_USERNAME") ?? "guest",
                Password = Environment.GetEnvironmentVariable("MESSAGING_PASSWORD") ?? "guest",
                VHost = Environment.GetEnvironmentVariable("MESSAGING_VHOST") ?? "/",
            };
            
            var apiKey = Environment.GetEnvironmentVariable("OPEN_WEATHER_API_KEY") ?? "eb8b1a9405e659b2ffc78f0a520b1a46";
            
            var service = new OpenWeatherService(new WebClientRequester(), apiKey);
            var messaging = new Messaging(messagingConfig);
            var consumer = new WeatherConsumer(service, messaging);
            messaging.Register(consumer);
            Console.WriteLine("Mensageria configurada");
            Console.WriteLine("Assim que começar a entrar mensagens, este consumidor irá consumi-las e jogalas novamente para as filas do Rabbit, conforme README.");
        }
    }
}