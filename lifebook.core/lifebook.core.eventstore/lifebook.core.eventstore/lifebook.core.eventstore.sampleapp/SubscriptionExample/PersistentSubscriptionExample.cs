using System;
using System.Threading.Tasks;
using Castle.Windsor;
using lifebook.core.eventstore.domain.models;
using lifebook.core.eventstore.subscription.Apis;
using lifebook.core.eventstore.subscription.Interfaces;

namespace lifebook.core.eventstore.sampleapp.SubscriptionExample
{
    public class PersistentSubscriptionExample : IExample
    {
        private readonly StreamCategorySpecifier category = new StreamCategorySpecifier("test", "primary", "TestPerson");

        public void Run(string[] args, WindsorContainer container)
        {
            // install the componenets
            new EventStoreSubscriptionTestIntaller().Install(container);
            var sut = container.Resolve<IEventStoreSubscription>();
            sut.SubscribeToSingleStream<EntityEvent>(category, async se => {
                Console.WriteLine(se);
                Console.WriteLine(se.Event.EventName);
            });
            Console.ReadLine();
        }
    }
}
