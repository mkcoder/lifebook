using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using lifebook.core.eventstore.domain.interfaces;
using lifebook.core.eventstore.extensions;

namespace lifebook.core.eventstore.services
{
    public class EventStoreClient : AbstractEventStoreClient
    {
        private IEventStoreConnection eventStoreConnection;

        public object EventVersion { get; private set; }

        public EventStoreClient()
        {
            // IEventStoreClientConnection
            // IEventStoreConfiguration
            eventStoreConnection = EventStoreConnection.Create(ConnectionSettings.Create(), new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1113));
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

        internal override List<Event> ReadEvent(StreamCategorySpecifier specifier)
        {
            var slice = eventStoreConnection.ReadStreamEventsForwardAsync(specifier.GetCategoryStream(), 0, int.MaxValue, true).Result;
            return null;
        }

        internal override async Task<List<AggregateEvent>> ReadEventsAsync(StreamCategorySpecifier specifier)
        {
            var result = new List<AggregateEvent>();
            var reading = true;
            int index = 0;
            int readPerCycle = 200;            
            do
            {
                var slice = await eventStoreConnection.ReadStreamEventsForwardAsync($"$ce-{specifier.GetCategoryStream()}", index*readPerCycle, readPerCycle, true);
                result.AddRange(
                    slice.Events
                    .Select(e => AggregateEvent.Create(e.Event.EventType, e.Event.Created, e.Event.Data, e.Event.Metadata))
                    .ToList()
                );
                reading = !slice.IsEndOfStream;
                index++;
            } while (reading);

            return result;
        }

        [Obsolete]
        internal override void WriteEvent(StreamCategorySpecifier specifier, Event e)
        {
                WriteEventAsync(specifier, e).Wait();
        }

        internal override async Task WriteEventAsync(StreamCategorySpecifier specifier, Event e)
        {
            await eventStoreConnection.AppendToStreamAsync(specifier.GetCategoryStreamWithAggregateId(e.EntityId),
                e.EventNumber == 0 ? ExpectedVersion.Any : e.EventNumber,
                new EventData(e.EventId, e.GetEventType(), true, e.EventDataToByteArray(), e.EventMetadataToByteArray()));
        }
    }
}
