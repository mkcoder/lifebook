using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using lifebook.core.cqrses.Domains;
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
            var deepCopy = JObject.FromObject(aggregateEvent).DeepClone().ToObject<AggregateEvent>();
            deepCopy.Data = new eventstore.domain.models.Data(ObjectToByteArray(Data));
            deepCopy.EventName = EventName;
            deepCopy.EventVersion = EventVersion;
            return deepCopy;
        }

        private static byte[] ObjectToByteArray(Object obj)
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