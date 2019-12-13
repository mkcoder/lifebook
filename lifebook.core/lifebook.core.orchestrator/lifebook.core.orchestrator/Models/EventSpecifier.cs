using lifebook.core.eventstore.domain.models;

namespace lifebook.core.orchestrator.Models
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
