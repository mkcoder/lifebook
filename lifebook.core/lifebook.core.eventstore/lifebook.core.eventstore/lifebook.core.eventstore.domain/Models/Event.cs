using System;
using System.Reflection;
using System.Text;
using System.Text.Json;
using lifebook.core.eventstore.domain.api;

namespace lifebook.core.eventstore.domain.models
{
    public abstract class Event : IEvent
    {
        public Guid EntityId { get; set; }
        public Guid EventId { get; set; } = Guid.NewGuid();
        public Guid CorrelationId { get; set; } 
        public Guid CausationId { get; set; } = Guid.NewGuid();
        public string EventName { get; set; }
        public int EventVersion { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public virtual string EventType { get; set; } = "EntityEvent";

        public Event()
        {
            EventName = GetType().Name;
            CorrelationId = CorrelationId != Guid.Empty ? CorrelationId : Guid.NewGuid();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"[{base.ToString()}]");
            sb.Append(JsonSerializer.Serialize(this, GetType()));
            return sb.ToString();
        }
    }
}
