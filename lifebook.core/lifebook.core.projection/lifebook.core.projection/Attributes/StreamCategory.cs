using System;
using System.Reflection.Metadata;

namespace lifebook.core.projection.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class StreamCategory : Attribute
    {
        public string StreamName { get; }
        public string InstanceName { get; }
        public string Aggregate { get; }

        public StreamCategory(string aggregate, string streamName, string instanceName="Primary")
        {
            Aggregate = aggregate;
            StreamName = streamName;
            InstanceName = instanceName;
        }
    }
}
