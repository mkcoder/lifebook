using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using lifebook.core.messagebus.Interfaces;
using lifebook.core.messagebus.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
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

        public void Subscribe<T>(MessageQueueInformation broker, Action<EventBusMessage<T>> action)
        {
            var consumer = new EventingBasicConsumer(_model);
            consumer.Received += (obj, evt) => {
                var body = (T)Convert.ChangeType(ByteArrayToObject(evt.Body), typeof(T));
                var msg = new EventBusMessage<T>(body)
                {
                    CorrelationId = evt.BasicProperties.CorrelationId,
                    MessageName = evt.BasicProperties.MessageId,
                    ExchangeName = evt.Exchange,
                    RoutingKey = evt.RoutingKey,
                    Redelivered = evt.Redelivered
                };
                action.Invoke(msg);
            };
            _model.BasicConsume(broker.QueueName, true, consumer);
        }

        public static object ByteArrayToObject(byte[] arrBytes)
        {
            using (var memStream = new MemoryStream())
            {
                object result = null;
                try
                {
                    result = Encoding.UTF8.GetString(arrBytes);
                    return result;
                }
                catch(Exception)
                {
                    var binForm = new BinaryFormatter();
                    memStream.Write(arrBytes, 0, arrBytes.Length);
                    memStream.Seek(0, SeekOrigin.Begin);
                    memStream.Position = 0;
                    try
                    {
                        result = binForm.Deserialize(memStream);
                        return result;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }                    
                }
            }
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
