namespace lifebook.core.eventstore.domain.interfaces
{
    public class StreamCategorySpecifier
    {
        public string Service { get; }
        public string Instance { get; }
        public string Category { get; }
        public string AggregateId { get; }

        public StreamCategorySpecifier(string service, string instance, string category) : this(service, instance, category, null)
        {
        }

        public StreamCategorySpecifier(string service, string instance, string category, string aggregateId)
        {
            Service = service;
            Instance = instance;
            Category = category;
            AggregateId = aggregateId;
        }

        public string GetCategoryStream() => $"{Service}.{Instance}.{Category}";
        public string GetCategoryStreamWithAggregateId() => $"{Service}.{Instance}.{Category}-{AggregateId}";
    }
}