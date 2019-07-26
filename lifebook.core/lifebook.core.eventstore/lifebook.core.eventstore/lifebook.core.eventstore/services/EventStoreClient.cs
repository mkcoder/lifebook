using System;
using System.Collections.Generic;
using lifebook.core.eventstore.domain.interfaces;

namespace lifebook.core.eventstore.services
{
    public class EventStoreClient : AbstractEventStoreClient
    {
        private Dictionary<string, List<IEvent>> EventStore = new Dictionary<string, List<IEvent>>();

        internal override void WriteEvent(StreamCategorySpecifier specifier, IEvent @e)
        {
            if (!EventStore.ContainsKey(specifier.GetCategoryStream())) EventStore[specifier.GetCategoryStream()] = new List<IEvent>();
            EventStore[specifier.GetCategoryStream()].Add(@e);
        }

        internal override List<IEvent> ReadEvent(StreamCategorySpecifier specifier)
        {
            if (EventStore.ContainsKey(specifier.GetCategoryStream()))
            {
                return EventStore[specifier.GetCategoryStream()];
            }

            throw new ArgumentNullException($"Could not find any stream for this {specifier}.");
        }
    }
}
