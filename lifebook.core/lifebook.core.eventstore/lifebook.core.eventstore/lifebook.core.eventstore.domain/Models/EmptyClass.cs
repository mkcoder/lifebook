namespace lifebook.core.eventstore.domain.models
{
    public class EventSpecifier
    {
        public string EventName { get; }
        public StreamCategorySpecifier StreamCategorySpecifier { get; }

        public EventSpecifier(string eventName, StreamCategorySpecifier streamCategorySpecifier)
        {
            EventName = eventName;
            StreamCategorySpecifier = streamCategorySpecifier;
        }
    }
}
