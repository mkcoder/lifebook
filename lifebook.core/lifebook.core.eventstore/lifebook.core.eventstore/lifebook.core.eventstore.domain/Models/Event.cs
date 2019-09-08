using System;
using System.Reflection;
using lifebook.core.eventstore.domain.api;

namespace lifebook.core.eventstore.domain.models
{
    public abstract class Event : IEvent
    {
        public Guid EntityId { get; set; }
        public Guid EventId { get; set; } = Guid.NewGuid();
        public Guid CorrelationId { get; set; } = Guid.NewGuid();
        public Guid CausationId { get; set; } = Guid.NewGuid();
        public string EventName { get; set; }
        public int EventVersion { get; set; }
        public DateTime DateCreated { get; set; }

        public Event()
        {
            EventName = GetType().Name;
        }
    }
}
