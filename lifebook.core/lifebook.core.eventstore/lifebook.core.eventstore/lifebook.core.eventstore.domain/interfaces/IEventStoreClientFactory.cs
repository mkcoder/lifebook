﻿using System;
namespace lifebook.core.eventstore.domain.interfaces
{
    public interface IEventStoreClientFactory
    {
        IEventStoreClient GetEventStoreClient();
        IEventStoreClient GetFakeEventStoreClient();
    }
}