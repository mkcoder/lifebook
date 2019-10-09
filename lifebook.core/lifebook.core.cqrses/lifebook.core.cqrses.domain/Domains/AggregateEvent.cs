using System;
using System.Text;
using System.Text.Json;
using lifebook.core.eventstore.domain.api;
using lifebook.core.eventstore.domain.Attributes;
using lifebook.core.eventstore.domain.models;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace lifebook.core.cqrses.Domains
{
    public class AggregateEvent : Event
    {
        [Metadata]
        public string CommandName { get; set; }
        [JsonIgnore]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Data Data { get; set; }

        public AggregateEvent()
        {
            EventType = "AggregateEvent";
        }
    }

    public class AggregateEventCreator : ICreateEvent<AggregateEvent>
    {
        public string CommandName { get; set; }

        public AggregateEvent Create(string eventType, DateTime created, byte[] data, byte[] metadata)
        {
            var ag = JsonSerializer.Deserialize<AggregateEvent>(Encoding.UTF8.GetString(metadata));
            ag.Data = new Data(data);
            return ag;
        }
    }
}
