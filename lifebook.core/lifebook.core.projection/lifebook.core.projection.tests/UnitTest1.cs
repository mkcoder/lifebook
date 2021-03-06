using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Elasticsearch.Net.Specification.SecurityApi;
using lifebook.core.cqrses.Domains;
using lifebook.core.projection.Attributes;
using lifebook.core.projection.Domain;
using lifebook.core.projection.Interfaces;
using lifebook.core.projection.Ioc;
using lifebook.core.projection.Services;
using lifebook.core.projection.Services.StreamTracker;
using lifebook.core.services;
using lifebook.core.services.extensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace lifebook.core.projection.tests
{
    public class PersonEntity : EntityProjection
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
    }

    [StreamCategory("Person", "person", "primary")]
    public class PersonProjector : Projector<PersonEntity>
    {
        public PersonProjector(ProjectorServices projectorServices) : base(projectorServices)
        {
        }

        [UponEvent("PersonCreated")]
        public void PersonCreated(AggregateEvent aggregateEvent)
        {
            var personCreated = aggregateEvent.Data.TransformDataFromString(str => JObject.Parse(str));
        }

        public void Run(bool createContext = false, IWindsorContainer container = null)
        {
            if(createContext)
            {
                base.Run(container);
            }
            Start();
        }
    }

    public class Tests
    {
        IWindsorContainer container = new WindsorContainer();

        [SetUp]
        public void Setup()
        {
            var assembly = GetType().Assembly.GetRootAssembly();
            container.Install(
                FromAssembly.InThisApplication(typeof(ServiceInstaller).Assembly),                
                FromAssembly.InThisApplication(assembly)
            );
            var projector = new PersonProjector(container.Resolve<ProjectorServices>());
            projector.Run(true, container);
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}