using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vigo.Service.DTO.Shared;

namespace vigo.Service.EmailAuthenModule
{
    public class EmailAuthenProducer
    {
        private readonly ConnectionFactory _connectionFactory;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private static EmailAuthenProducer? instance;
        private static readonly object lockObject = new object();
        private EmailAuthenProducer()
        {
            _connectionFactory = new ConnectionFactory
            {
                Uri = new Uri("amqp://admin:admin@rabbitmq:5672")
            };
            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare("email-authen-queue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
        }

        public static EmailAuthenProducer Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new EmailAuthenProducer();
                        }
                    }
                }
                return instance;
            }
        }

        public void SendEmailAuthen(EmailAuthenDTO dto)
        {
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(dto));
            _channel.BasicPublish("", "email-authen-queue", null, body);
        }
    }
}
