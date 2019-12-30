using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using lifebook.core.messagebus.Interfaces;
using lifebook.core.messagebus.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Framing;

namespace lifebook.core.messagebus.Services
{
    public class RabbitMqMessageBus : IMessageBus
    {
        private IModel _model;
        private readonly MessageQueueInformation _queueInformation;

        public RabbitMqMessageBus(IModel model, MessageQueueInformation queueInformation)
        {
            _model = model;
            _queueInformation = queueInformation;
        }

        public MessageConfirmation Publish(object message)
        {
            var body = ConvertObjectToByteArray(message);
            _model.BasicPublish(_queueInformation.QueueName, _queueInformation.RoutingKey, body: body);
            return MessageConfirmation.Ok();
        }

        public T Subscribe<T>(MessageQueueInformation broker)
        {
            return null;
        }

        private byte[] ConvertObjectToByteArray(Object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }
    }
}
