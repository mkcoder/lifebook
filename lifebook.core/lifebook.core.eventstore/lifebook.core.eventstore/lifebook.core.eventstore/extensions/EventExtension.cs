using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using lifebook.core.eventstore.domain.api;
using lifebook.core.eventstore.domain.Attributes;
using lifebook.core.eventstore.domain.models;
using Newtonsoft.Json.Linq;

namespace lifebook.core.eventstore.extensions
{
    public static class EventExtension
    {
        public static byte[] EventDataToByteArray(this IEvent e)
        {
            var properties = typeof(Event).GetProperties(BindingFlags.Instance | BindingFlags.Public).Select(p => p.Name).ToArray();
            var additionalProperties = e.GetType()
                                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                .Where(p => p.GetCustomAttribute(typeof(MetadataAttribute)) != null)
                                .Select(p => p.Name)
                                .Union(properties)
                                .ToArray();
            var json = JObject.FromObject(e);
            foreach (var elem in json.DeepClone().ToObject<JObject>())
            {
                if (additionalProperties.Contains(elem.Key))
                {
                    json.Remove(elem.Key);
                }
            }
            return Encoding.UTF8.GetBytes(json.ToString());
        }

        public static byte[] EventMetadataToByteArray(this IEvent e)
        {
            var properties = typeof(Event)
                                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                .Select(p => p.Name)
                                .ToArray();
            var additionalProperties = e.GetType()
                                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                .Where(p => p.GetCustomAttribute(typeof(MetadataAttribute)) != null)
                                .Select(p => p.Name)
                                .Union(properties)
                                .ToArray();
            var json = JObject.FromObject(e);
            foreach (var elem in json.DeepClone().ToObject<JObject>())
            {
                if(!additionalProperties.Contains(elem.Key))
                {
                    json.Remove(elem.Key);
                }
            }
            return Encoding.UTF8.GetBytes(json.ToString());
        }

        public static EntityEvent AsAggregateEvent(this IEvent e)
            => e as EntityEvent;

        public static string GetEventType(this IEvent e)
        {
            return "AggregateEvent";
        }
    }
}
