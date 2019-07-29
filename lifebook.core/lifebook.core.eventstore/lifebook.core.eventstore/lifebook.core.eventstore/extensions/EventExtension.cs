using System.IO;
using System.Reflection;
using System.Text;
using System.Text.Json;
using lifebook.core.eventstore.domain.interfaces;

namespace lifebook.core.eventstore.extensions
{
    public static class EventExtension
    {
        public static byte[] EventDataToByteArray(this IEvent e)
        {
            var result = JsonSerializer.Serialize(e, e.GetType());
            return Encoding.UTF8.GetBytes(result);
        }

        public static byte[] EventMetadataToByteArray(this IEvent e)
        {
            var properties = typeof(Event).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var json = "";
            using (var stream = new MemoryStream())
            {
                using (var writer = new Utf8JsonWriter(stream))
                {
                    writer.WriteStartObject();
                    foreach (var property in properties)
                    {
                        writer.WriteString(property.Name, property.GetValue(e, null).ToString());
                    }
                    writer.WriteEndObject();
                    writer.Flush();
                }
                json = Encoding.UTF8.GetString(stream.ToArray());
            }
            return Encoding.UTF8.GetBytes(json);
        }
    }
}
