using System;
using System.Collections.Generic;

namespace lifebook.core.eventstore.domain.interfaces
{
    public interface IEventReader
    {
        List<Event> ReadAllEventsFromStreamCategory(StreamCategorySpecifier categorySpecifier);
        List<Event> ReadAllEventsFromStreamCategoryForAggregate(StreamCategorySpecifier categorySpecifier);
        Event GetLastEventWrittenToStream(StreamCategorySpecifier streamCategory);
        Event GetLastEventWrittenToStreamForAggregate(StreamCategorySpecifier streamCategory);
    }
}
