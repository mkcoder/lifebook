using System;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace lifebook.core.eventstore.domain.interfaces
{
    public interface IEvent
    {
    }

    public abstract class Event : IEvent
    {
        [JsonIgnore]
        [IgnoreDataMember]
        public Guid AggregateId { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public Guid EventId { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public int Version { get; set; }
    }

    public sealed class AggregateEvent : Event
    {
        public DateTime DateCreated { get; private set; }
        public object Data { get; set; }

        internal AggregateEvent() { }

        public static AggregateEvent Create(string eventType, long eventNumber, DateTime created, Guid eventId, byte[] data, byte[] metadata)
        {
            var type = Type.GetType(eventType);
            var ag = JsonSerializer.Deserialize<AggregateEvent>(Encoding.UTF8.GetString(metadata));
            ag.DateCreated = created;
            ag.Data = JsonSerializer.Deserialize(Encoding.UTF8.GetString(metadata), type);
            return ag;
        }
    }
}
