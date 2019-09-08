using System;
using System.Text;
using System.Text.Json;
using lifebook.core.eventstore.domain.api;
using lifebook.core.eventstore.domain.Attributes;
using lifebook.core.eventstore.domain.models;
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
            person = new PersonCreated
            {
                EntityId = Guid.Parse("2795a3b4-864c-48df-a894-4d07ba6e49fc"),
                EventId = Guid.Parse("b24533e1-5054-4ab7-8498-f847ef18a717"),
                EventVersion = -1,
                Name = "Bob",
                CommandName = "CreatePerson"
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
        public void Extension_Serialize_Metadata_HasCorrectValues()
        {
            var result = JsonSerializer.Deserialize<PersonCreated>(metadata);
            Assert.AreEqual(Guid.Parse("b24533e1-5054-4ab7-8498-f847ef18a717"), result.EventId);
            Assert.AreEqual("PersonCreated", result.EventName);
            Assert.AreEqual(-1, result.EventVersion);
            Assert.AreNotEqual(Guid.Empty, result.CorrelationId);
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
        public void Extension_Serialize_Metadata_DeSearizeable_WithMetadata_Shows()
        {
            var result = JsonSerializer.Deserialize<PersonCreated>(metadata);
            Assert.IsNotNull(result.CommandName);
            Assert.AreEqual("CreatePerson", result.CommandName);
        }

        [Test]
        public void Extension_Serialize_Data_DeSearizeable()
        {
            var result = JsonSerializer.Deserialize<PersonCreated>(data);
            
            Assert.IsNotNull(result.Name);
            Assert.AreEqual(Guid.Empty, result.EntityId);
        }

        [Test]
        public void Extension_Serialize_Data_DeSearizeable_WithMetadata_Hides_Metadata()
        {
            var result = JsonSerializer.Deserialize<PersonCreated>(data);

            Assert.IsNull(result.CommandName);
        }
    }

    class PersonCreated : Event
    {
        [Metadata]
        public string CommandName { get; set; }
        public string Name { get; set; }
    }
}
