using System.Collections.Generic;
using System.Threading.Tasks;
using lifebook.core.eventstore.domain.models;

namespace lifebook.core.eventstore.domain.api
{
    public interface IEventReader
    {
        Task<List<TOut>> ReadAllEventsFromSingleStreamCategoryAsync<T, TOut>(StreamCategorySpecifier categorySpecifier) where T : ICreateEvent<TOut>, new() where TOut: Event;
        Task<List<TOut>> ReadAllEventsFromStreamCategoryAsync<T, TOut>(StreamCategorySpecifier categorySpecifier) where T : ICreateEvent<TOut>, new() where TOut: Event;
        Task<List<EntityEvent>> ReadAllEventsFromStreamCategoryAsync(StreamCategorySpecifier categorySpecifier);
        Task<List<EntityEvent>> ReadAllEventsFromStreamCategoryForAggregateAsync(StreamCategorySpecifier categorySpecifier);
        Task<Event> GetLastEventWrittenToStreamAsync(StreamCategorySpecifier streamCategory);
        Task<Event> GetLastEventWrittenToStreamForAggregateAsync(StreamCategorySpecifier streamCategory);
    }
}
