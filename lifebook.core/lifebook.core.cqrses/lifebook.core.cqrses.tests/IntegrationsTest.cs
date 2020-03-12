using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Unicode;
using System.Threading.Tasks;
using lifebook.core.cqrs.tests;
using lifebook.core.services.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace lifebook.core.cqrses.tests
{
    public class Tests
    {
        IntegrationServer server;
        Guid AID = Guid.Parse("96f90b52-f1c6-4d45-9701-bee0e5a6e900");

        [SetUp]
        public async Task Setup()
        {
            server = await IntegrationServer.Start<Program>();
        }

        [Test]
        public async Task Test1()
        {
            await server.TestApi(async api =>
            {
                var json = new JObject()
                {
                    ["AggregateType"]= "Person",
	                ["CommandName"]= "CreatePerson",
	                ["CommandId"]= Guid.NewGuid(),
	                ["AggregateId"]= AID,
	                ["CorrelationId"]= "3fcc3a81-5e48-449a-8921-2579c246c389",
	                ["FirstName"]= "John",
	                ["LastName"]= "Doe",
	                ["Age"]= 23
                };
                var content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
                var result = await api.PostAsync("/CreatePerson", content);
                Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            });
        }
    }
}