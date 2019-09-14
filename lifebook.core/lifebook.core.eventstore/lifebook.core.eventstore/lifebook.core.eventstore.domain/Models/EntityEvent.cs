using System;
using System.Text;
using System.Text.Json;
using lifebook.core.eventstore.domain.api;

namespace lifebook.core.eventstore.domain.models
{
    public class EntityEvent : Event, ICreateEvent<EntityEvent>
    {
        public Data Data { get; protected set; }

        public EntityEvent Create(string eventType, DateTime created, byte[] data, byte[] metadata)
        {
            var ag = JsonSerializer.Deserialize<EntityEvent>(Encoding.UTF8.GetString(metadata));
            ag.DateCreated = created;
            ag.Data = new Data(data);
            return ag;
        }
    }
}
