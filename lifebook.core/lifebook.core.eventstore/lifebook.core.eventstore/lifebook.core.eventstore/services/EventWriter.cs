using System;
using lifebook.core.eventstore.domain.interfaces;

namespace lifebook.core.eventstore.services
{
    public class EventWriter : IEventWriter
    {
        private readonly AbstractEventStoreClient _eventStoreClient;

        public EventWriter(AbstractEventStoreClient abstractEventStoreClient)
        {
            _eventStoreClient = abstractEventStoreClient;
        }

        public void WriteEvent(StreamCategorySpecifier streamCategorySpecifier, IEvent @e)
        {
            _eventStoreClient.WriteEvent(streamCategorySpecifier, e);
        }
    }
}
