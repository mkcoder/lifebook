using lifebook.core.eventstore.domain.interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Tests
{
    public class EventStoreClientTest
    {
        private IEventStoreClient _sut;

        [SetUp]
        public void Setup()
        {
            _sut = Substitute.For<IEventStoreClient>();
        }
    }
}