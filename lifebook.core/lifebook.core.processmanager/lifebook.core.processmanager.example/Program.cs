using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using lifebook.core.eventstore.configurations;
using lifebook.core.eventstore.domain.api;
using lifebook.core.eventstore.domain.models;
using lifebook.core.eventstore.services;
using lifebook.core.processmanager.Services;
using lifebook.core.processmanager.Syntax;
using lifebook.core.services;
using lifebook.core.services.extensions;
using Newtonsoft.Json.Linq;
using AssemblyExtensions = lifebook.core.services.extensions.AssemblyExtensions;

namespace lifebook.core.processmanager.example
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var container = new WindsorContainer();
            container.Install(FromAssembly.InThisApplication(AssemblyExtensions.GetRootAssembly(typeof(Program).Assembly)));
            if (!container.Kernel.HasComponent(typeof(IEventStoreClient)))
            {
                container.Register(
                        Component.For<AbstractEventStoreClient>()
                        .ImplementedBy<EventStoreClient>()
                            .OnCreate(async es => await es.ConnectAsync())
                            .OnDestroy(async es => es.Close())
                            .LifeStyle.Singleton,
                        Component.For<IEventStoreClientFactory>().AsFactory(),
                        Component.For<IEventStoreClient>()
                            .OnCreate(async es => await es.ConnectAsync())
                            .OnDestroy(async es => es.Close())
                            .ImplementedBy<EventStoreClient>()
                            .Named("EventStoreClient").LifeStyle.Singleton,
                        Component.For<IEventWriter>().ImplementedBy<EventWriter>().LifeStyle.Transient,
                        Component.For<IEventReader>().ImplementedBy<EventReader>().LifeStyle.Transient
                    );
            }
            container.Register(Component.For<IWindsorContainer>().Instance(container).LifestyleSingleton());
            var pm = new DemoPersonProcess(container.Resolve<ProcessManagerServices>());
            await pm.Run();

            var t = new Thread(() => { });
            t.Start();
            Console.ReadLine();
        }
    }

    public class DemoPersonProcess : ProcessManager
    {
        public DemoPersonProcess(ProcessManagerServices processManagerServices) : base(processManagerServices)
        {
        }

        public override ProcessManagerConfiguration GetConfiguration()
        {
            return ProcessManagerConfigurationBuilder
                    .Instance
                    .UponEvent(new EventSpecifier("TestPersonCreated", new StreamCategorySpecifier("test", "primary", "TestPerson")))
                    .SetStepDescription("Set person age to 0")
                    .TakeAction(async evt => {
                        if (evt == null)
                        {
                            Console.WriteLine($"evt is null");
                        }
                        else
                        {
                        }

                        if (ViewBag == null)
                        {
                            Console.WriteLine("Viewbag is null");
                        }
                        else
                        {
                            ViewBag.HelloOtherProcess = "hello";
                        }
                    })
                    .AndThen()
                    .UponEvent(new EventSpecifier("TestPersonNameChanged", new StreamCategorySpecifier("test", "primary", "TestPerson")))
                    .SetStepDescription("Change person name to bob")
                    .TakeAction(async evt => {
                        if (evt == null)
                        {
                            Console.WriteLine($"evt is null");
                        }
                        else
                        {
                        }

                        if (ViewBag == null)
                        {
                            Console.WriteLine("Viewbag is null");
                        }
                        else
                        {
                            await WriteCustomEventAsync("CustomEvent", evt, new JObject()
                            {
                                ["MyName"] = "bob",
                                ["ViewBag"] = ViewBag
                            });
                            ViewBag["Id"] = Guid.NewGuid();
                        }
                    })
                    .AndThen()
                    .UponEvent(new EventSpecifier("CustomEvent", new StreamCategorySpecifier(ThisService, ThisInstance, ThisCategory)))
                    .SetStepDescription("Custom event happened")
                    .TakeAction(async evt => {
                        if (evt == null)
                        {
                            Console.WriteLine($"evt is null");
                        }
                        else
                        {
                            Debugger.Break();
                            Console.WriteLine("EVENT:");
                            Console.WriteLine(evt);
                        }

                        if (ViewBag == null)
                        {
                            Console.WriteLine("Viewbag is null");
                        }
                        else
                        {
                            ViewBag.Id2 = Guid.NewGuid();
                            Console.WriteLine(ViewBag);
                            Console.WriteLine(ViewBag["HelloOtherProcess"]);
                        }
                    })
                    .Configuration;
        }

        public async Task Run()
        {
            await Start();
        }
    }
}
