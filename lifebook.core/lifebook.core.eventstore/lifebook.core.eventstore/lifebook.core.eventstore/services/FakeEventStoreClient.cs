﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using lifebook.core.eventstore.domain.interfaces;

namespace lifebook.core.eventstore.services
{
    public class FakeEventStoreClient : AbstractEventStoreClient
    {
        private static ConcurrentDictionary<string, List<IEvent>> EventStore = new ConcurrentDictionary<string, List<IEvent>>();

        internal override void WriteEvent(StreamCategorySpecifier specifier, IEvent @e)
        {
            lock (EventStore)
            {
                if (!EventStore.ContainsKey(specifier.GetCategoryStream())) EventStore[specifier.GetCategoryStream()] = new List<IEvent>();
                EventStore[specifier.GetCategoryStream()].Add(@e);
            }
        }

        internal override List<IEvent> ReadEvent(StreamCategorySpecifier specifier)
        {
            lock (EventStore)
            {
                if (EventStore.ContainsKey(specifier.GetCategoryStream()))
                {
                    return EventStore[specifier.GetCategoryStream()];
                }
            }

            throw new ArgumentNullException($"Could not find any stream for this {specifier}.");
        }

        public override void Connect() => _connected = true;

        public override async Task ConnectAsync()
        {            
            await Task.Run(() => _connected = true);
        }
    }
}
