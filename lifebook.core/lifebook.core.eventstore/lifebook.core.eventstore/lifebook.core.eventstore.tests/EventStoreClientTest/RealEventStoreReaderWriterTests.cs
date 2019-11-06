using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using lifebook.core.eventstore.domain.api;
using lifebook.core.eventstore.domain.models;
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

            for (int i = 0; i < 5; i++)
            {
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
                    Age = new Random().Next(20)
                },
                new TestPersonAgeChanged()
                {
                    Age = new Random().Next(60)
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
                    Occupation = "maanager " + Guid.NewGuid()
                },
                new TestPersonOccupationChanged()
                {
                    Occupation = "developer" + Guid.NewGuid()
                },
                new TestPersonOccupationChanged()
                {
                    Occupation = "cto" + Guid.NewGuid()
                },
            };
            
                Guid g = Guid.NewGuid();
                var category = new StreamCategorySpecifier("test", "primary", "TestPerson", g);
                foreach (var e in lifeEvents)
                {
                    e.EntityId = g;
                    await eventWriter.WriteEventAsync(category, e);
                }
            }
        }

        [Explicit]
        [Test]
        public async Task Test2()
        {
            Guid productId = Guid.Parse("62e27ca5-6ad8-4832-99a2-5dbd849383fc"); // Guid.Parse("6022727c-7bfe-44f6-a722-48e1b98397df");
            var category = new StreamCategorySpecifier("test", "primary", "Catalog", productId);
            var catalogProductCreated = new TestProductCreated()
            {
                ProductName = "Product-5dbd849383fc"
            };
            catalogProductCreated.EntityId = productId;
            await eventWriter.WriteEventAsync(category, catalogProductCreated);
            Guid productPurchased = Guid.Parse("9cf88a45-1399-4e2f-9c67-e4dcf2cf5696");
            var personCateogry = new StreamCategorySpecifier("test", "primary", "TestPerson", productPurchased);
            var testProductPurchased = new TestProductPurchased()
            {
                ProductId = productId                
            };
            testProductPurchased.EntityId = productPurchased;
            await eventWriter.WriteEventAsync(personCateogry, testProductPurchased);
        }

        [Test]
        [Explicit]
        public async Task ReadIsWorking()
        {
            var categpory = new StreamCategorySpecifier("test", "primary", "TestPerson");
            var events = await eventReader.ReadAllEventsFromStreamCategoryAsync(categpory);
            var ae = events.FirstOrDefault(f => f.EventName == "TestPersonCreated").AsAggregateEvent();
            var e = ae.Data.TransformDataFromString(s => JObject.Parse(s).ToObject<PersonCreated>());
            Assert.IsNotNull(events);
            Assert.IsNotNull(e.EntityId);
        }
    }

    public class TestPersonCreated : Event
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Occupation { get; set; }
        public override string EventType { get; set; } = "TestEvent";
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

    public class TestProductPurchased : Event
    {
        public Guid ProductId { get; set; }
    }

    public class TestProductCreated : Event
    {
        public string ProductName { get; set; }
    }
}