using lifebook.core.cqrses.Domains;
using lifebook.core.projection.Attributes;
using lifebook.core.projection.Domain;
using lifebook.core.projection.Interfaces;
using lifebook.core.projection.Services;
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
        public PersonProjector(IProjectionStore projectionStore) : base(projectionStore)
        {
        }

        [UponEvent("PersonCreated")]
        public void PersonCreated(AggregateEvent aggregateEvent)
        {
            var personCreated = aggregateEvent.Data.TransformDataFromString(str => JObject.Parse(str));
        }
    }

    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}