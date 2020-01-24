using System;
using System.Collections.Generic;
using lifebook.core.messagebus.Interfaces;
using lifebook.core.messagebus.Models;
using lifebook.core.services.interfaces;
using RabbitMQ.Client;

namespace lifebook.core.messagebus.Services
{
    public class RabbitMqMessageBroker : IMessageBroker
    {
        private readonly IConfiguration _configuration;
        private ConnectionFactory factory = null;
        private IConnection connection;
        private bool _connected = false;

        public bool IsConnected => _connected;

        public RabbitMqMessageBroker(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Connect()
        {
            factory = new ConnectionFactory()
            {
                //UserName = _configuration.TryGetValueOrDefault("RabbitMqUsername", "qa"),
                //Password = _configuration.TryGetValueOrDefault("RabbitMqPassword", "123"),
                HostName = _configuration["RabbitMqHostName"],
                Port = _configuration.TryGetValueOrDefault("RabbitMqPort", 15672)
            };
            connection = factory.CreateConnection();
            _connected = true;           
        }

        public IMessageBus TryConnectingDirectlyToQueue(MessageQueueInformation messageQueueInformation)
        {
            if (_connected)
            {
                var model = connection.CreateModel();
                model.QueueDeclare(messageQueueInformation.QueueName,
                        autoDelete: false
                );
                    
                return new RabbitMqMessageBus(model, messageQueueInformation);
            }
            throw new InvalidOperationException("Connection must be made before creating a queue");
        }
    }
}
