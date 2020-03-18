using System;
using System.Linq;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MicroServiceBase.Communication
{
    public class Messaging : IMessage
    {
        private readonly MessagingConfiguration _configuration;
        private IConnection _connection;
        private ConnectionFactory _factory;

        public Messaging(MessagingConfiguration configuration)
        {
            _configuration = configuration;
            _factory = new ConnectionFactory
            {
                HostName = _configuration.Host,
                Port = _configuration.Port,
                UserName = _configuration.UserName,
                Password =  _configuration.Password,
                VirtualHost = _configuration.VHost,
            };
            _connection = _factory.CreateConnection();
        }

        private void Declare(IModel channel, string name)
        {
            channel.ExchangeDeclare(exchange: name, type: "direct");
            channel.QueueDeclare(queue: name,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            channel.QueueBind(name, name, "");
        }

        public void Publish<T>(T obj)
        {
            if (obj == null)
                throw new ArgumentNullException();
            
            var exchange = obj.GetType().Name;
            var channel = _connection.CreateModel();
            Declare(channel, exchange);

            var content = JsonSerializer.Serialize(obj);
            var body = Encoding.UTF8.GetBytes(content);
            channel.BasicPublish(exchange: exchange,
                routingKey: "",
                basicProperties: null,
                body: body);
        }

        public void Register<T>(IMessageConsumer<T> consumer)
        {
            if (consumer == null)
                throw new ArgumentNullException();

            var type = consumer.GetType().GetInterfaces().FirstOrDefault()?.GenericTypeArguments.FirstOrDefault();
            if (type == null)
                throw new ArgumentNullException();

            var queue = type.Name;
            var channel = _connection.CreateModel();
            Declare(channel, queue);
            
            var eventConsumer = new EventingBasicConsumer(channel);
            eventConsumer.Received += (model, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body);
                T obj = JsonSerializer.Deserialize<T>(content);
                consumer.Receive(obj);
            };
            
            channel.BasicConsume(queue: queue,
                autoAck: true,
                consumer: eventConsumer);
        }
    }
}