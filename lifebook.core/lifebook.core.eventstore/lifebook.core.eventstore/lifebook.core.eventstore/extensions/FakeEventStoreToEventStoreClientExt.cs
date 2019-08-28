using lifebook.core.eventstore.domain.api;
using lifebook.core.eventstore.services;

namespace lifebook.core.eventstore.extensions
{
    public static class FakeEventStoreToEventStoreClientExt
    {
        public static IEventStoreClient GetFakeEventStoreClient(this IEventStoreClientFactory eventStoreClientFactory)
        {
            return new FakeEventStoreClient();
        }
    }
}
