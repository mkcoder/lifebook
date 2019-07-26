using System;
using System.Collections.Generic;

namespace lifebook.core.eventstore.domain.interfaces
{
    public interface IEventReader
    {
        List<IEvent> ReadAllEventsFromStreamCategory(StreamCategorySpecifier categorySpecifier);
        List<IEvent> ReadAllEventsFromStreamCategoryForAggregate(StreamCategorySpecifier categorySpecifier);
        IEvent GetLastEventWrittenToStream(StreamCategorySpecifier streamCategory);
        IEvent GetLastEventWrittenToStreamForAggregate(StreamCategorySpecifier streamCategory);
    }
}
