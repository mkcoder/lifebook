using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using lifebook.core.eventstore.domain.api;
using lifebook.core.eventstore.domain.models;
using lifebook.core.logging.interfaces;

namespace lifebook.core.eventstore.services
{
    public class EventWriter : IEventWriter
    {
        private readonly AbstractEventStoreClient _eventStoreClient;
        private readonly ILogger _logger;

        public EventWriter(AbstractEventStoreClient abstractEventStoreClient, ILogger logger)
        {
            _eventStoreClient = abstractEventStoreClient;
            _logger = logger;
        }

        public async Task WriteEventAsync(StreamCategorySpecifier streamCategorySpecifier, Event e)
        {
            try
            {
                await _eventStoreClient.WriteEventAsync(streamCategorySpecifier, e);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error occured trying to write event {e.EventName} to stream. {e.ToString()}");
                throw;
            }
        }

        public async Task WriteEventAsync(StreamCategorySpecifier streamCategorySpecifier, Event e, byte[] data,byte[] metadata=null)
        {
            try
            {
                await _eventStoreClient.WriteEventAsync(streamCategorySpecifier, e, data, metadata);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error occured trying to write event {e.EventName} to stream. {e.ToString()}");
                throw;
            }
        }

        public async Task WriteEventAsync(StreamCategorySpecifier streamCategorySpecifier, List<Event> e)
        {
            foreach (var item in e)
            {
                await WriteEventAsync(streamCategorySpecifier, item);
            }
        }
    }
}
