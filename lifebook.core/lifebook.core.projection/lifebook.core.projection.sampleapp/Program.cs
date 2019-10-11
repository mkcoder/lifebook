using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using lifebook.core.cqrses.Domains;
using lifebook.core.logging.ioc;
using lifebook.core.projection.Attributes;
using lifebook.core.projection.Domain;
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

        public void Run()
        {
            Start();
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            IWindsorContainer container = new WindsorContainer();

            var assembly = typeof(Program).Assembly.GetRootAssembly();
            container.Register(
                Component.For<DbContextOptions<DatabaseProjectionStore>>().Instance(new DbContextOptions<DatabaseProjectionStore>()),
                Component.For<DbContextOptions<EntityStreamTracker>>().Instance(new DbContextOptions<EntityStreamTracker>())
            );
            container.Install(
                FromAssembly.InThisApplication(typeof(BootLoader).Assembly),
                FromAssembly.InThisApplication(assembly)
            );
            var projector = new PersonProjector(container.Resolve<ProjectorServices>());
            projector.Run();            
            Console.ReadLine();
        }
    }
}
