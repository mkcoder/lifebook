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

        public IEvent GetLastEventWrittenToStream(StreamCategorySpecifier streamCategory)
            => _eventStoreClient.ReadEvent(streamCategory).Last();

        public IEvent GetLastEventWrittenToStreamForAggregate(StreamCategorySpecifier streamCategory)
            => ReadAllEventsFromStreamCategoryForAggregate(streamCategory).Last();

        public List<IEvent> ReadAllEventsFromStreamCategory(StreamCategorySpecifier categorySpecifier)
            => _eventStoreClient.ReadEvent(categorySpecifier);

        public List<IEvent> ReadAllEventsFromStreamCategoryForAggregate(StreamCategorySpecifier categorySpecifier)
            => _eventStoreClient.ReadEvent(categorySpecifier)
                                    .Where(e => e.AggregateId == categorySpecifier.AggregateId)
                                    .ToList();
    }
}
