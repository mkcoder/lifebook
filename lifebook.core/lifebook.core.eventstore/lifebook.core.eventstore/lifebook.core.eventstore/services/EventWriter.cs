using System;
using System.Threading.Tasks;
using lifebook.core.eventstore.domain.api;

namespace lifebook.core.eventstore.services
{
    public class EventWriter : IEventWriter
    {
        private readonly AbstractEventStoreClient _eventStoreClient;

        public EventWriter(AbstractEventStoreClient abstractEventStoreClient)
        {
            _eventStoreClient = abstractEventStoreClient;
        }

        public void WriteEvent(StreamCategorySpecifier streamCategorySpecifier, Event @e)
        {
            try
            {
                _eventStoreClient.Connect();
                _eventStoreClient.WriteEvent(streamCategorySpecifier, e);
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

        public async Task WriteEventAsync(StreamCategorySpecifier streamCategorySpecifier, Event e)
        {
            try
            {
                if(!_eventStoreClient.IsConnected)
                {
                    await _eventStoreClient.ConnectAsync();
                }
                await _eventStoreClient.WriteEventAsync(streamCategorySpecifier, e);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
            }
        }
    }
}
