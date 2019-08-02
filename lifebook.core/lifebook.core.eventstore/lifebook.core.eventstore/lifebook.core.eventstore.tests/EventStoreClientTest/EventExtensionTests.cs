using System;
using System.Text;
using System.Text.Json;
using lifebook.core.eventstore.domain.api;
using lifebook.core.eventstore.extensions;
using NUnit.Framework;

namespace lifebook.core.eventstore.tests.EventStoreClientTest
{
    public class EventExtensionTests
    {
        private readonly byte[] metadata;
        private readonly byte[] data;
        private readonly PersonCreated person;

        public EventExtensionTests()
        {
            person = new PersonCreated()
            {
                EntityId = Guid.Parse("2795a3b4-864c-48df-a894-4d07ba6e49fc"),
                EventId = Guid.Parse("b24533e1-5054-4ab7-8498-f847ef18a717"),
                EventNumber = -1,
                Name = "Bob"
            };
            metadata = person.EventMetadataToByteArray();
            data = person.EventDataToByteArray();
        }

        [Test]
        public void Extension_Serialize_Metadata_Data()
        {
            Assert.IsNotNull(metadata);
            Assert.IsNotNull(data);
        }

        [Test]
        public void Extension_Serialize_Metadata_DoesNotHaveDataProperties()
        {
            var result = JsonSerializer.Serialize(person);
            Assert.AreNotSame(result, metadata);
        }

        [Test]
        public void Extension_Serialize_Metadata_DeSearizeable()
        {
            var result = JsonSerializer.Deserialize<PersonCreated>(metadata);
            Assert.IsNotNull(result.EntityId);
            Assert.IsNull(result.Name);
        }

        [Test]
        public void Extension_Serialize_Data_DeSearizeable()
        {
            var result = JsonSerializer.Deserialize<PersonCreated>(data);
            
            Assert.IsNotNull(result.Name);
            Assert.AreEqual(Guid.Empty, result.EntityId);
        }
    }

    class PersonCreated : Event
    {
        public string Name { get; set; }
    }
}
