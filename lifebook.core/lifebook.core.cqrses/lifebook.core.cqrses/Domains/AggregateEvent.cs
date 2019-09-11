using System;
using System.Text;
using System.Text.Json;
using lifebook.core.eventstore.domain.api;
using lifebook.core.eventstore.domain.Attributes;
using lifebook.core.eventstore.domain.models;
using Newtonsoft.Json;

namespace lifebook.core.cqrses.Domains
{
    public class AggregateEvent : Event
    {
        [Metadata]
        public string CommandName { get; set; }
        public new string EventType { get; set; } = "AggregateEvent";
        [Newtonsoft.Json.JsonIgnore]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Data Data { get; set; }
    }

    public class AggregateEventCreator : ICreateEvent<AggregateEvent>
    {
        public string CommandName { get; set; }

        public AggregateEvent Create(string eventType, DateTime created, byte[] data, byte[] metadata)
        {
            var ag = System.Text.Json.JsonSerializer.Deserialize<AggregateEvent>(Encoding.UTF8.GetString(metadata));
            /*var aee = new AggregateEntityEvent();
            aee.EntityId = ag.EntityId;
            aee.CorrelationId = ag.CorrelationId;
            aee.CommandName = ag.CommandName;
            aee.DateCreated = created;
            aee.EventName = ag.EventName;
            aee.EventVersion = ag.EventVersion;
            aee.CausationId = ag.CausationId;
            aee.Data = new Data(data);*/
            ag.Data = new Data(data);
            return ag;
        }
    }
}
