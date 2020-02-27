using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using lifebook.core.cqrses.Domains;
using lifebook.core.eventstore.domain.models;
using Newtonsoft.Json.Linq;

namespace lifebook.core.processmanager.Aggregates
{
    internal class ModelEvent
    {
        public string EventName { get; internal set; }
        public int EventVersion { get; internal set; }
        public JObject Data { get; internal set; }

        internal AggregateEvent Merge(AggregateEvent aggregateEvent)
        {
            var deepCopy = JObject.FromObject(aggregateEvent).ToObject<AggregateEvent>();
            deepCopy.Data = new Data(Encoding.UTF8.GetBytes(Data.ToString()));
            deepCopy.EventName = EventName;
            deepCopy.EventVersion = 0;
            return deepCopy;
        }

        internal (AggregateEvent ae, byte[] data) CommitEvent(AggregateEvent aggregateEvent)
        {

            var deepCopy = JObject.FromObject(aggregateEvent).ToObject<AggregateEvent>();
            var bytes = Encoding.UTF8.GetBytes(Data.ToString());
            deepCopy.EventId = Guid.NewGuid();
            deepCopy.Data = new Data(bytes);
            deepCopy.EventName = EventName;
            deepCopy.EventVersion = 0;
            return (deepCopy, bytes);
        }

        private static byte[] ObjectToByteArray(object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }
    }
}