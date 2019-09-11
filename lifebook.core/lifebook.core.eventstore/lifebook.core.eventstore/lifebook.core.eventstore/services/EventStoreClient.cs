using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using lifebook.core.eventstore.configurations;
using lifebook.core.eventstore.domain.api;
using lifebook.core.eventstore.domain.models;
using lifebook.core.eventstore.extensions;
using Microsoft.Extensions.Configuration;

namespace lifebook.core.eventstore.services
{
    public class EventStoreClient : AbstractEventStoreClient
    {
        private readonly IEventStoreConnection eventStoreConnection;
        private readonly EventStoreConfiguration _eventStoreConfiguration;

        public object EventVersion { get; private set; }

        public EventStoreClient(EventStoreConfiguration eventStoreConfiguration)
        {
            eventStoreConnection = EventStoreConnection.Create(ConnectionSettings.Create(), new IPEndPoint(IPAddress.Parse(eventStoreConfiguration.IpAddress), eventStoreConfiguration.Port));
            _eventStoreConfiguration = eventStoreConfiguration;
        }

        public override void Connect()
        {
            eventStoreConnection.ConnectAsync().Wait();
            _connected = true;
        }

        public override async Task ConnectAsync()
        {
            await eventStoreConnection.ConnectAsync().ConfigureAwait(false);
            _connected = true;
        }

        public override void Close()
        {
            eventStoreConnection.Close();
            _connected = false;
        }

        internal override async Task<List<EntityEvent>> ReadEventsAsync(StreamCategorySpecifier specifier)
        {
            var result = new List<EntityEvent>();
            var reading = true;
            int index = 0;
            int readPerCycle = 200;            
            do
            {
                var slice = await eventStoreConnection.ReadStreamEventsForwardAsync($"$ce-{specifier.GetCategoryStream()}", index*readPerCycle, readPerCycle, true);
                result.AddRange(
                    slice.Events
                    .Select(e => EntityEvent.Create(e.Event.EventType, e.Event.Created, e.Event.Data, e.Event.Metadata))
                    .ToList()
                );
                reading = !slice.IsEndOfStream;
                index++;
            } while (reading);

            return result;
        }

        internal override async Task<List<TOut>> ReadEventsAsync<T, TOut>(StreamCategorySpecifier specifier)
        {
            var result = new List<TOut>();
            var reading = true;
            int index = 0;
            int readPerCycle = _eventStoreConfiguration.ReadPerCycle;
            do
            {
                var slice = await eventStoreConnection.ReadStreamEventsForwardAsync($"$ce-{specifier.GetCategoryStream()}", index * readPerCycle, readPerCycle, true);
                result.AddRange(
                    slice.Events
                    .Select(e => new T().Create(e.Event.EventType, e.Event.Created, e.Event.Data, e.Event.Metadata))
                    .ToList()
                );
                reading = !slice.IsEndOfStream;
                index++;
            } while (reading);

            return result;
        }

        internal override async Task WriteEventAsync(StreamCategorySpecifier specifier, Event e)
        {
            await eventStoreConnection.AppendToStreamAsync(specifier.GetCategoryStreamWithAggregateId(),
                e.EventVersion == 0 ? ExpectedVersion.Any : e.EventVersion,
                new EventData(e.EventId, e.EventType, true, e.EventDataToByteArray(), e.EventMetadataToByteArray()));
        }
    }
}
