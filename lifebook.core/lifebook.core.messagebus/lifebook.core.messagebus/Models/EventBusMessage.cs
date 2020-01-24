using System;
namespace lifebook.core.messagebus.Models
{
    public class EventBusMessage<T>
    {
        public T Data { get; internal set; }
        public string CorrelationId { get; internal set; }
        public string MessageName { get; internal set; }
        public string ExchangeName { get; internal set; }
        public string RoutingKey { get; internal set; }
        public bool Redelivered { get; internal set; }

        public EventBusMessage(T body)
        {
            this.Data = body;
        }
    }
}
