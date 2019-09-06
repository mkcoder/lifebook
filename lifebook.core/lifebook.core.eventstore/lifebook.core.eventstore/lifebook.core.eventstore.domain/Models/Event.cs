using System;
using lifebook.core.eventstore.domain.api;

namespace lifebook.core.eventstore.domain.models
{
    public abstract class Event : IEvent
    {
        public Guid EntityId { get; set; }
        public Guid EventId { get; set; } = Guid.NewGuid();
        public Guid CorrelationId { get; set; } = Guid.NewGuid();
        public Guid CausationId { get; set; } = Guid.NewGuid();
        public long EventNumber { get; set; } 
        public string EventName { get; set; }
        public string CommandName { get; set; }
        public int EventVersion { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
