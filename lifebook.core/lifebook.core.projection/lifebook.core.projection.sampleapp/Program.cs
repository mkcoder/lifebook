using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using lifebook.core.cqrses.Domains;
using lifebook.core.logging.ioc;
using lifebook.core.projection.Attributes;
using lifebook.core.projection.Domain;
using lifebook.core.projection.Interfaces;
using lifebook.core.projection.Services;
using lifebook.core.projection.Services.StreamTracker;
using lifebook.core.services.extensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace lifebook.core.projection.sampleapp
{
    public class PersonEntity : EntityProjection
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Occupation { get; set; }
    }

    // test.primary.TestPerson
    [StreamCategory("TestPerson", "test", "primary")]
    public class PersonProjector : Projector<PersonEntity>
    {
        public PersonProjector(ProjectorServices projectorServices) : base(projectorServices)
        {
        }

        [UponEvent("TestPersonCreated")]
        public void PersonCreated(AggregateEvent aggregateEvent)
        {
            var personCreated = aggregateEvent.Data.TransformDataFromString(str => JObject.Parse(str));
            Value.FirstName = personCreated["FirstName"].ToObject<string>();
            Value.LastName = personCreated["LastName"].ToObject<string>();
            Value.Age = personCreated["Age"].ToObject<int>();
            Value.Occupation = personCreated["Occupation"].ToObject<string>();
        }

        [UponEvent("TestPersonAgeChanged")]
        public void TestPersonAgeChanged(AggregateEvent aggregateEvent)
        {
            var personCreated = aggregateEvent.Data.TransformDataFromString(str => JObject.Parse(str));
            Value.Age = personCreated["Age"].ToObject<int>();
        }

        [UponEvent("TestPersonOccupationChanged")]
        public void TestPersonOccupationChanged(AggregateEvent aggregateEvent)
        {
            var personCreated = aggregateEvent.Data.TransformDataFromString(str => JObject.Parse(str));
            Value.Occupation = personCreated["Occupation"].ToObject<string>();
        }

        [UponEvent("TestPersonNameChanged")]
        public void TestPersonNameChanged(AggregateEvent aggregateEvent)
        {
            var personCreated = aggregateEvent.Data.TransformDataFromString(str => JObject.Parse(str));
            Value.FirstName = personCreated["FirstName"].ToObject<string>();
            Value.LastName = personCreated["LastName"].ToObject<string>();
        }

        public void Run()
        {
            Start();
        }
    }

    public class PersonGuidToOccupation : EntityProjection
    {
        public string Occupation { get; set; }
    }

    // test.primary.TestPerson
    [StreamCategory("TestPerson", "test", "primary")]
    public class PersonGuidToOccupationProjector : Projector<PersonGuidToOccupation>
    {
        public PersonGuidToOccupationProjector(ProjectorServices projectorServices) : base(projectorServices)
        {
        }

        [UponEvent("TestPersonCreated")]
        public void PersonCreated(AggregateEvent aggregateEvent)
        {
            var personCreated = aggregateEvent.Data.TransformDataFromString(str => JObject.Parse(str));
            Value.Occupation = personCreated["Occupation"].ToObject<string>();
        }

        [UponEvent("TestPersonOccupationChanged")]
        public void TestPersonOccupationChanged(AggregateEvent aggregateEvent)
        {
            var personCreated = aggregateEvent.Data.TransformDataFromString(str => JObject.Parse(str));
            Value.Occupation = personCreated["Occupation"].ToObject<string>();
        }

        public void Run()
        {
            Start();
        }
    }

    public class PersonGuidToName : EntityProjection
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    // test.primary.TestPerson
    [StreamCategory("TestPerson", "test", "primary")]
    public class PersonGuidToNameProjector : Projector<PersonGuidToName>
    {
        public PersonGuidToNameProjector(ProjectorServices projectorServices) : base(projectorServices)
        {
        }

        [UponEvent("TestPersonCreated")]
        public void PersonCreated(AggregateEvent aggregateEvent)
        {
            var personCreated = aggregateEvent.Data.TransformDataFromString(str => JObject.Parse(str));
            Value.FirstName = personCreated["FirstName"].ToObject<string>();
            Value.LastName = personCreated["LastName"].ToObject<string>();
        }

        [UponEvent("TestPersonNameChanged")]
        public void TestPersonNameChanged(AggregateEvent aggregateEvent)
        {
            var personCreated = aggregateEvent.Data.TransformDataFromString(str => JObject.Parse(str));
            Value.FirstName = personCreated["FirstName"].ToObject<string>();
            Value.LastName = personCreated["LastName"].ToObject<string>();
        }
    }

    public class Program
    {
        static async Task Main(string[] args)
        {
            // Hosting
            IWindsorContainer container = new WindsorContainer();
            await Hosting.Run(container);
            Console.ReadLine();
        }
    }

    public class Hosting
    {
        public static async Task Run(IWindsorContainer container)
        {
            var assembly = typeof(Program).Assembly.GetRootAssembly();

            container.Install(
                FromAssembly.InThisApplication(typeof(BootLoader).Assembly),
                FromAssembly.InThisApplication(assembly)
            );

            var contextCreator = container.Resolve<IApplicationContextCreator>();
            contextCreator.CreateContext();
            var projectors = container.ResolveAll<IProjector>();

            projectors.Select(p => p.TestProjector()).ToList().ForEach(s => s.Run());
        }
    }

    public static class TestProjectorExt
    {
        public static TestProjector TestProjector(this IProjector projector)
        {
            return new TestProjector(projector);
        }
    }

    public class TestProjector
    {
        private readonly IProjector _projector;

        public TestProjector(IProjector projector)
        {
            _projector = projector;
        }

        public void Run()
        {
            var projector = _projector.GetType().GetMethod("Start", BindingFlags.NonPublic | BindingFlags.Instance);
            projector?.Invoke(_projector, null);
        }
    }
}
