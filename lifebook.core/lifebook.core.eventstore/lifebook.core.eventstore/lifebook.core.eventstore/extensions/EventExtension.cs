using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using lifebook.core.eventstore.domain.interfaces;
using Newtonsoft.Json.Linq;

namespace lifebook.core.eventstore.extensions
{
    public static class EventExtension
    {
        public static byte[] EventDataToByteArray(this IEvent e)
        {
            var properties = typeof(Event).GetProperties(BindingFlags.Instance | BindingFlags.Public).Select(p => p.Name).ToArray();
            var json = JObject.FromObject(e);
            foreach (var elem in json.DeepClone().ToObject<JObject>())
            {
                if (properties.Contains(elem.Key))
                {
                    json.Remove(elem.Key);
                }
            }
            return Encoding.UTF8.GetBytes(json.ToString());
        }

        public static byte[] EventMetadataToByteArray(this IEvent e)
        {
            var properties = typeof(Event).GetProperties(BindingFlags.Instance | BindingFlags.Public).Select(p => p.Name).ToArray();
            var json = JObject.FromObject(e);
            foreach (var elem in json.DeepClone().ToObject<JObject>())
            {
                if(!properties.Contains(elem.Key))
                {
                    json.Remove(elem.Key);
                }
            }
            json["EventName"] = e.GetType().Name;
            return Encoding.UTF8.GetBytes(json.ToString());
        }

        public static AggregateEvent AsAggregateEvent(this IEvent e)
            => e as AggregateEvent;

        public static string GetEventType(this IEvent e)
        {
            return "AggregateEvent";
        }
    }
}
