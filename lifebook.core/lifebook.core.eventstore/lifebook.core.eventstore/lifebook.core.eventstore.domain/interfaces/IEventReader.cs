using System.Collections.Generic;
using System.Threading.Tasks;

namespace lifebook.core.eventstore.domain.interfaces
{
    public interface IEventReader
    {
        Task<List<Event>> ReadAllEventsFromStreamCategoryAsync(StreamCategorySpecifier categorySpecifier);
        Task<List<Event>> ReadAllEventsFromStreamCategoryForAggregateAsync(StreamCategorySpecifier categorySpecifier);
        Task<Event> GetLastEventWrittenToStreamAsync(StreamCategorySpecifier streamCategory);
        Task<Event> GetLastEventWrittenToStreamForAggregateAsync(StreamCategorySpecifier streamCategory);
    }
}
