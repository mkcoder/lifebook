using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lifebook.core.eventstore.domain.api;
using lifebook.core.eventstore.domain.models;

namespace lifebook.core.eventstore.services
{
    public class EventReader : IEventReader
    {
        private readonly AbstractEventStoreClient _eventStoreClient;

        public EventReader(AbstractEventStoreClient eventStoreClient)
        {
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

        // NO NO NO NO NO
        [Obsolete]
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
            try
            {
                await _eventStoreClient.ConnectAsync();
                return await func();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                _eventStoreClient.Close();
            }
        }
    }
}
