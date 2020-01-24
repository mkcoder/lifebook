using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using lifebook.core.messagebus.Interfaces;
using lifebook.core.messagebus.Models;
using Newtonsoft.Json.Linq;
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
            var json = JObject.FromObject(message);
            var body = Encoding.UTF8.GetBytes(json.ToString());
            var properties = _model.CreateBasicProperties();
            properties.CorrelationId = (string)(json["CorrelationId"] ?? Guid.NewGuid());
            properties.MessageId = message.GetType().Name;
            properties.Persistent = true;
            _model.BasicPublish(
                exchange: "",
                routingKey: _queueInformation.RoutingKey,                
                basicProperties: properties,
                body: body
            );
            return MessageConfirmation.Ok();
        }

        public void Subscribe<T>(MessageQueueInformation broker, Action<EventBusMessage<T>> action)
        {
            var consumer = new EventingBasicConsumer(_model);
            consumer.Received += (obj, evt) => {
                var evtBody = Encoding.UTF8.GetString(evt.Body);
                T data = default(T);
                if (evtBody.IsJson())
                {
                    data = JObject.Parse(evtBody).ToObject<T>();
                }
                else
                {
                    data = (T)Convert.ChangeType(evtBody, typeof(T));
                }
                
                var msg = new EventBusMessage<T>(data)
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
    }

    public static class Extensions
    {
        public static bool IsJson(this string jsonString)
        {
            try
            {
                JObject.Parse(jsonString);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
