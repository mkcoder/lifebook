using System;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace lifebook.core.eventstore.domain.api
{
    public interface IEvent
    {
    }

    public abstract class Event : IEvent
    {
        public Guid EntityId { get; set; }
        public Guid EventId { get; set; } = Guid.NewGuid();
        public Guid CorrelationId { get; set; } = Guid.NewGuid();
        public long EventNumber { get; set; } 
        public string EventName { get; set; }
    }

    public sealed class AggregateEvent : Event
    {
        public DateTime DateCreated { get; private set; }
        public Data Data { get; private set; }

        internal AggregateEvent() { }

        public static AggregateEvent Create(string eventType, DateTime created, byte[] data, byte[] metadata)
        {
            var ag = JsonSerializer.Deserialize<AggregateEvent>(Encoding.UTF8.GetString(metadata));
            ag.DateCreated = created;
            ag.Data = new Data(data);
            return ag;
        }
    }

    public sealed class Data
    {
        private byte[] _bytes;

        public Data(byte[] bytes)
        {
            _bytes = bytes;
        }

        public T TransformDataFromByte<T>(Func<byte[], T> transformer)
        {
            return transformer(_bytes);
        }

        public T TransformDataFromString<T>(Func<string, T> transformer)
        {
            return transformer(Encoding.UTF8.GetString(_bytes));
        }
    }
}
