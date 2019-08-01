using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using lifebook.core.eventstore.domain.interfaces;
using lifebook.core.eventstore.extensions;
using lifebook.core.eventstore.testing.framework;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace lifebook.core.eventstore.tests.EventStoreClientTest
{
    public class RealEventStoreReaderWriterTests : BaseServiceTests<EventStoreClientTestIntaller>
    {
        private readonly IEventReader eventReader;
        private readonly IEventWriter eventWriter;

        public RealEventStoreReaderWriterTests()
        {
            eventReader = container.Resolve<IEventReader>();
            eventWriter = container.Resolve<IEventWriter>();
        }

        [Explicit]
        [Test]
        public async Task WriteIsWorking()
        {
            var personGuid = Guid.Parse("5b4a440e-db6d-40d4-8b9a-a0cc2cccd16d");
            var lifeEvents = new List<Event>() {
                new TestPersonCreated()
                {
                    Id = 1,
                    FirstName = "Jake",
                    LastName = "Blitz",
                    Age = 24,
                    Occupation = "Developer"
                },
                new TestPersonAgeChanged()
                {
                    Age = 25
                },
                new TestPersonAgeChanged()
                {
                    Age = 26
                },
                new TestPersonNameChanged()
                {
                    FirstName = "James",
                    LastName = "Blitz",
                },
                new TestPersonNameChanged()
                {
                    FirstName = "James",
                    LastName = "Aver",
                },
                new TestPersonOccupationChanged()
                {
                    Occupation = "maanager"
                },
                new TestPersonOccupationChanged()
                {
                    Occupation = "developer"
                },
                new TestPersonOccupationChanged()
                {
                    Occupation = "cto"
                },
            };
            
            for (int i = 0; i < 5; i++)
            {
                Guid g = Guid.NewGuid();
                var categpory = new StreamCategorySpecifier("test", "primary", "TestPerson", g);
                foreach (var e in lifeEvents)
                {
                    e.EntityId = g;
                    await eventWriter.WriteEventAsync(categpory, e);
                }
            }
        }

        [Test]
        public async Task ReadIsWorking()
        {
            var categpory = new StreamCategorySpecifier("test", "primary", "TestPerson");
            var events = await eventReader.ReadAllEventsFromStreamCategoryAsync(categpory);
            var ae = events.FirstOrDefault(f => f.EventName == "TestPersonCreated").AsAggregateEvent();
            var e = ae.Data.TransformDataFromString(s => JObject.Parse(s).ToObject<PersonCreated>());
            Assert.IsNotNull(events);
            Assert.IsNotNull(e.EntityId);
        }

        [Test]
        public async Task ExceptionClosesStream()
        {
            var categpory = new StreamCategorySpecifier("test", "primary", "Exception");
            var events = await eventReader.ReadAllEventsFromStreamCategoryAsync(categpory);
        }
    }

    public class TestPersonCreated : Event
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Occupation { get; set; }
    }

    public class TestPersonAgeChanged : Event
    {
        public int Age { get; set; }
    }

    public class TestPersonNameChanged : Event
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class TestPersonOccupationChanged : Event
    {
        public string Occupation { get; set; }
    }
}
