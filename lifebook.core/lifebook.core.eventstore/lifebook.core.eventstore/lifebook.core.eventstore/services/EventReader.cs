using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lifebook.core.eventstore.domain.api;

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
            try
            {
                await _eventStoreClient.ConnectAsync();
                return (await _eventStoreClient.ReadEventsAsync(streamCategory)).Last();
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

        public async Task<Event> GetLastEventWrittenToStreamForAggregateAsync(StreamCategorySpecifier streamCategory)
        {
            try
            {
                await _eventStoreClient.ConnectAsync();
                return (await ReadAllEventsFromStreamCategoryForAggregateAsync(streamCategory)).Last();
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

        public async Task<List<Event>> ReadAllEventsFromStreamCategoryAsync(StreamCategorySpecifier categorySpecifier)            
        {
            try
            {
                await _eventStoreClient.ConnectAsync();
                var result = await _eventStoreClient.ReadEventsAsync(categorySpecifier);
                return result.ToList<Event>();
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

        public async Task<List<Event>> ReadAllEventsFromStreamCategoryForAggregateAsync(StreamCategorySpecifier categorySpecifier)
        {            
            try
            {
                await _eventStoreClient.ConnectAsync();
                return (await _eventStoreClient.ReadEventsAsync(categorySpecifier))
                        .Where(e => e.EntityId == categorySpecifier.AggregateId)
                        .ToList<Event>();
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
