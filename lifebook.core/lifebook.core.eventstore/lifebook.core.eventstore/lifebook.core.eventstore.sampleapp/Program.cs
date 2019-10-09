using System;
using Castle.Windsor;
using lifebook.core.eventstore.sampleapp.SubscriptionExample;

namespace lifebook.core.eventstore.sampleapp
{
    class Program
    {
        static void Main(string[] args)
        {
            WindsorContainer container = new WindsorContainer();
            var ps = new PersistentSubscriptionExample();
            ps.Run(args, container);

        }
    }
}
