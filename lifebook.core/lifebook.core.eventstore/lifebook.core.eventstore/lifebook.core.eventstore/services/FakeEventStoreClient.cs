using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lifebook.core.eventstore.domain.api;
using lifebook.core.eventstore.domain.models;
using lifebook.core.eventstore.extensions;

namespace lifebook.core.eventstore.services
{
    public class FakeEventStoreClient : AbstractEventStoreClient
    {
        private static ConcurrentDictionary<string, List<(Event, DateTime)>> EventStore = new ConcurrentDictionary<string, List<(Event, DateTime)>>();

        public override void Connect() => _connected = true;

        public override async Task ConnectAsync()
        {
            await Task.Run(() => _connected = true);
        }

        public override void Close()
        {
            _connected = false;
        }

        internal override async Task WriteEventAsync(StreamCategorySpecifier specifier, Event e)
        {
            await WriteEvent(specifier, e);
        }

        private async Task WriteEvent(StreamCategorySpecifier specifier, Event @e)
        {
            lock (EventStore)
            {
                if (!EventStore.ContainsKey(specifier.GetCategoryStream()))
                    EventStore[specifier.GetCategoryStream()] = new List<(Event, DateTime)>();
                EventStore[specifier.GetCategoryStream()].Add((@e, DateTime.Now));                
            }

            await Task.CompletedTask;
        }

        internal async Task<List<(Event, DateTime)>> ReadEvent(StreamCategorySpecifier specifier)
        {
            lock (EventStore)
            {
                if (EventStore.ContainsKey(specifier.GetCategoryStream()))
                {
                    return EventStore[specifier.GetCategoryStream()];
                }
            }
            await Task.CompletedTask;
            throw new ArgumentNullException($"Could not find any stream for this {specifier}.");
        }

        internal override async Task<List<EntityEvent>> ReadEventsAsync(StreamCategorySpecifier specifier)
        {
            return (await ReadEvent(specifier)).Select(e => new EntityEvent().Create("AggregateEvent", e.Item2, e.Item1.EventDataToByteArray(), e.Item1.EventMetadataToByteArray())).ToList();
        }

        internal override Task<List<TOut>> ReadEventsAsync<T, TOut>(StreamCategorySpecifier specifier)
        {
            throw new NotImplementedException();
        }

        internal override Task<List<EntityEvent>> ReadSingleStreamEventsAsync(StreamCategorySpecifier specifier)
        {
            throw new NotImplementedException();
        }

        internal override Task<List<TOut>> ReadSingeStreamEventsAsync<T, TOut>(StreamCategorySpecifier specifier)
        {
            throw new NotImplementedException();
        }

        internal override Task WriteEventAsync(StreamCategorySpecifier streamCategorySpecifier, Event e, byte[] data, byte[] metadata = null)
        {
            throw new NotImplementedException();
        }
    }
}
