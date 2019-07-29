using System;
using System.Runtime.Serialization;
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
}
