using System;
using System.Text;
using System.Text.Json;
using lifebook.core.eventstore.domain.api;
using lifebook.core.eventstore.domain.Attributes;
using lifebook.core.eventstore.domain.models;
using lifebook.core.eventstore.extensions;

namespace lifebook.core.cqrses.Domains
{
    public class AggregateEvent : Event
    {
        [Metadata]
        public string CommandName { get; set; }
    }

    public class AggregateEntityEvent : EntityEvent, ICreateEvent<AggregateEntityEvent>
    {
        public string CommandName { get; set; }

        public new AggregateEntityEvent Create(string eventType, DateTime created, byte[] data, byte[] metadata)
        {
            var aee = new AggregateEntityEvent();
            var ag = JsonSerializer.Deserialize<AggregateEvent>(Encoding.UTF8.GetString(metadata));
            return ag;
        }
    }
}
