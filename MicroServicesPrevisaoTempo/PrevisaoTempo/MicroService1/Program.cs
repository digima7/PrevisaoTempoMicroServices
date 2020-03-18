using System;
using MicroServiceBase.Communication;
using Nancy.Hosting.Self;
using Nancy.TinyIoc;

namespace MicroService1
{
    class Program
    {
        static void Main(string[] args)
        {
            var messagingConfig = new MessagingConfiguration 
            { 
                Host = Environment.GetEnvironmentVariable("MESSAGING_HOST") ?? "localhost",
                Port = Convert.ToInt32(Environment.GetEnvironmentVariable("MESSAGING_PORT") ?? "5672"),
                UserName = Environment.GetEnvironmentVariable("MESSAGING_USERNAME") ?? "guest",
                Password = Environment.GetEnvironmentVariable("MESSAGING_PASSWORD") ?? "guest",
                VHost = Environment.GetEnvironmentVariable("MESSAGING_VHOST") ?? "/",
            };

            var databaseConfig = new DatabaseConfig
            {
                Type = Environment.GetEnvironmentVariable("DATABASE_TYPE") ?? "sqlite",
                ConnectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING") ?? "DataSource=./database.db",
                // Type = "postgresql",
                // ConnectionString = "User ID=postgres;Password=postgres;Server=localhost;Port=5432;Database=postgres;Pooling=true;"
            };

            var waitTime = TimeSpan.FromSeconds(
                Convert.ToInt64(Environment.GetEnvironmentVariable("UPDATE_WEATHER_DELAY") ?? "20000"));

            Console.WriteLine(waitTime);
            
            var messaging = new Messaging(messagingConfig);
            var database = new WeatherDatabase(databaseConfig);
            var service = new WeatherUpdateService(waitTime, database, messaging);
            service.Start();

            messaging.Register(service);

            var controller = new WeatherController(database, service);

            var nancyBootstrapper = new WeatherBootstrap(controller);
            using (var nancyHost = new NancyHost(nancyBootstrapper, new Uri("http://localhost:8000/")))
            {
                nancyHost.Start();
                Console.WriteLine("Nancy now listening - navigating to http://localhost:8000/. Press enter to stop");
                Console.ReadKey();
            }
            Console.WriteLine("Stopped. Good bye!");
        }
    }
}