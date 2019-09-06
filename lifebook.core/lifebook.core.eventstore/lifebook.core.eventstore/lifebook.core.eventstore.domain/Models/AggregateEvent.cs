using System;
using System.Text;
using System.Text.Json;

namespace lifebook.core.eventstore.domain.models
{
    public sealed class AggregateEvent : Event
    {
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
}
