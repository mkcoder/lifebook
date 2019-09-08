using System;
using System.Text;
using System.Text.Json;

namespace lifebook.core.eventstore.domain.models
{
    public class EntityEvent : Event
    {
        public Data Data { get; private set; }

        public static EntityEvent Create(string eventType, DateTime created, byte[] data, byte[] metadata)
        {
            var ag = JsonSerializer.Deserialize<EntityEvent>(Encoding.UTF8.GetString(metadata));
            ag.DateCreated = created;
            ag.Data = new Data(data);
            return ag;
        }
    }
}
