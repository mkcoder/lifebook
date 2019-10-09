using System;
using System.Threading.Tasks;
using lifebook.core.eventstore.domain.models;
using lifebook.core.eventstore.subscription.Apis;
using lifebook.core.eventstore.subscription.Interfaces;
using lifebook.core.eventstore.subscription.Services;
using lifebook.core.eventstore.testing.framework;
using lifebook.core.logging.interfaces;
using NUnit.Framework;

namespace lifebook.core.eventstore.tests.SubscriptionTests
{
    public class SubscibingToStreamWorks : BaseServiceTests<EventStoreSubscriptionTestIntaller>
    {
        private readonly IEventStoreSubscription sut;

        private readonly StreamCategorySpecifier category = new StreamCategorySpecifier("test", "primary", "TestPerson");

        public SubscibingToStreamWorks()
        {
            sut = container.Resolve<IEventStoreSubscription>();
        }

        [Test]
        public void Works()
        {
            sut.SubscribeToSingleStream<EntityEvent>(category, react);            
        }

        private async Task react(SubscriptionEvent<EntityEvent> arg)
        {
            var a = arg;           
        }
    }
}
