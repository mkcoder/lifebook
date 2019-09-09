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

        public class AggregateEventCreator
        {
            public string EventName { get; private set; }
            public int Version { get; private set; }
            public Event EventData { get; private set; }

            private AggregateEventCreator(string eventName, int version)
            {
                EventName = eventName;
                Version = version;
            }

            public static AggregateEventCreator Create(string eventName, int version)
            {
                return new AggregateEventCreator(eventName, version);
            }

            public AggregateEventCreator WithData(Event @e)
            {
                EventData = e;
                return this;
            }

            public AggregateEvent Create() =>
                new AggregateEvent()
                {
                    EventName = EventName,
                    EventVersion = Version
                };
        }
    }

    public class AggregateEntityEvent : EntityEvent, ICreateEvent<AggregateEntityEvent>
    {
        public string CommandName { get; set; }

        public new AggregateEntityEvent Create(string eventType, DateTime created, byte[] data, byte[] metadata)
        {
            var ag = JsonSerializer.Deserialize<AggregateEvent>(Encoding.UTF8.GetString(metadata));
            var aee = new AggregateEntityEvent();
            aee.EntityId = ag.EntityId;
            aee.CorrelationId = ag.CorrelationId;
            aee.CommandName = ag.CommandName;
            aee.DateCreated = created;
            aee.EventName = ag.EventName;
            aee.EventVersion = ag.EventVersion;
            aee.CausationId = ag.CausationId;
            aee.Data = new Data(data);
            return aee;
        }
    }
}
