using System.Collections.Generic;
using System.Threading.Tasks;
using lifebook.core.eventstore.domain.models;

namespace lifebook.core.eventstore.domain.api
{
    public interface IEventReader
    {
        Task<List<T>> ReadAllEventsFromStreamCategoryAsync<T>(StreamCategorySpecifier categorySpecifier) where T : EntityEvent, ICreateEvent<T>, new();
        Task<List<Event>> ReadAllEventsFromStreamCategoryAsync(StreamCategorySpecifier categorySpecifier);
        Task<List<Event>> ReadAllEventsFromStreamCategoryForAggregateAsync(StreamCategorySpecifier categorySpecifier);
        Task<Event> GetLastEventWrittenToStreamAsync(StreamCategorySpecifier streamCategory);
        Task<Event> GetLastEventWrittenToStreamForAggregateAsync(StreamCategorySpecifier streamCategory);
    }
}
