using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lifebook.core.eventstore.domain.api;
using lifebook.core.eventstore.domain.models;
using lifebook.core.logging.interfaces;
using NeoSmart.AsyncLock;

namespace lifebook.core.eventstore.services
{
    public class EventReader : IEventReader
    {
        private readonly AbstractEventStoreClient _eventStoreClient;
        private readonly AsyncLock asyncLock = new AsyncLock();
        private readonly ILogger _logger;

        public EventReader(AbstractEventStoreClient eventStoreClient, ILogger logger)
        {
            _logger = logger;
            _eventStoreClient = eventStoreClient;
        }

        public async Task<Event> GetLastEventWrittenToStreamAsync(StreamCategorySpecifier streamCategory)
        {
            return await TryCatchCloseConnection(async () =>
            {
                return (await _eventStoreClient.ReadEventsAsync(streamCategory)).Last();
            });
        }

        public async Task<Event> GetLastEventWrittenToStreamForAggregateAsync(StreamCategorySpecifier streamCategory)
        {
            return await TryCatchCloseConnection(async () =>
            {
                return (await ReadAllEventsFromStreamCategoryForAggregateAsync(streamCategory)).Last();
            });
        }

        public async Task<List<Event>> ReadAllEventsFromStreamCategoryAsync(StreamCategorySpecifier categorySpecifier)            
        {
            return await TryCatchCloseConnection(async () =>
            {
                var result = await _eventStoreClient.ReadEventsAsync(categorySpecifier);
                return result.ToList<Event>();
            });            
        }

        public async Task<List<TOut>> ReadAllEventsFromStreamCategoryAsync<T, TOut>(StreamCategorySpecifier categorySpecifier) where T : ICreateEvent<TOut>, new() where TOut : Event
        {
            return await TryCatchCloseConnection(async () =>
            {
                var result = await _eventStoreClient.ReadEventsAsync<T, TOut>(categorySpecifier);
                return result;
            });
        }

        public async Task<List<Event>> ReadAllEventsFromStreamCategoryForAggregateAsync(StreamCategorySpecifier categorySpecifier)
        {
            return await TryCatchCloseConnection(async () =>
            {
                return (await _eventStoreClient.ReadEventsAsync(categorySpecifier))
                        .Where(e => e.EntityId == categorySpecifier.AggregateId)
                        .ToList<Event>();
            });
        }

        private async Task<T> TryCatchCloseConnection<T>(Func<Task<T>> func)
        {
            T result;
            try
            {
                result = await func();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error trying to read event from eventstore");
                throw;
            }
            return result;
        }
    }
}
