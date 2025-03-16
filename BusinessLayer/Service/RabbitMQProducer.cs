using BusinessLayer.Interface;
using RabbitMQ.Client;
using System.Text;

namespace BusinessLayer.Service
{
    public class RabbitMQProducer : IRabbitMQProducer
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly IConnection _connection;
        
        public RabbitMQProducer(IConnectionFactory connectionFactory, IConnection connection)
        {
            _connectionFactory = connectionFactory;
            _connection = connection;
        }

        public void PublishMessage(string message)
        {
            using (var connection = _connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "TestQueue",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "",
                                     routingKey: "TestQueue",
                                     basicProperties: null,
                                     body: body);
            }
        }
    }
}
