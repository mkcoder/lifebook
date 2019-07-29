using System;
using System.Collections.Generic;
using System.Linq;
using lifebook.core.eventstore.domain.interfaces;

namespace lifebook.core.eventstore.services
{
    public class EventReader : IEventReader
    {
        private readonly AbstractEventStoreClient _eventStoreClient;

        public EventReader(AbstractEventStoreClient eventStoreClient)
        {
            _eventStoreClient = eventStoreClient;
        }

        public Event GetLastEventWrittenToStream(StreamCategorySpecifier streamCategory)
            => _eventStoreClient.ReadEvent(streamCategory).Last();

        public Event GetLastEventWrittenToStreamForAggregate(StreamCategorySpecifier streamCategory)
            => ReadAllEventsFromStreamCategoryForAggregate(streamCategory).Last();

        public List<Event> ReadAllEventsFromStreamCategory(StreamCategorySpecifier categorySpecifier)
            => _eventStoreClient.ReadEvent(categorySpecifier);

        public List<Event> ReadAllEventsFromStreamCategoryForAggregate(StreamCategorySpecifier categorySpecifier)
            => _eventStoreClient.ReadEvent(categorySpecifier)
                                    .Where(e => e.AggregateId == categorySpecifier.AggregateId)
                                    .ToList();
    }
}
