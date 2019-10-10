using System;
using System.Reflection.Metadata;
using lifebook.core.eventstore.domain.models;

namespace lifebook.core.projection.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class StreamCategory : Attribute
    {
        public string StreamName { get; }
        public string InstanceName { get; }
        public string Aggregate { get; }

        public StreamCategory(string aggregate, string streamName, string instanceName="primary")
        {
            Aggregate = aggregate;
            StreamName = streamName;
            InstanceName = instanceName;
        }

        public override string ToString()
        {
            return $"{StreamName}.{InstanceName}.{Aggregate}";
        }

        internal StreamCategorySpecifier ToStreamCategorySpecifier()
        {
            return new StreamCategorySpecifier(StreamName, InstanceName, Aggregate);
        }
    }
}
