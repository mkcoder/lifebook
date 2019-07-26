﻿using System.Collections.Generic;
using lifebook.core.eventstore.domain.interfaces;

namespace lifebook.core.eventstore.services
{
    public abstract class AbstractEventStoreClient : IEventStoreClient
    {
        public abstract void Connect();

        public abstract bool IsConnected();

        internal abstract List<IEvent> ReadEvent(StreamCategorySpecifier specifier);

        internal abstract void WriteEvent(StreamCategorySpecifier specifier, IEvent @e);
    }
}
