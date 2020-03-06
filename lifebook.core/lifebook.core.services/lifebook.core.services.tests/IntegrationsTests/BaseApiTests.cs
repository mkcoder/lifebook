using System.Net;
using System.Threading.Tasks;
using lifebook.core.services.exampleapp;
using lifebook.core.services.Testing;
using NUnit.Framework;

namespace lifebook.core.services.tests.ApiTests
{
    public class BaseApiTests
    {
        IntegrationServer integrationServer;

        [OneTimeSetUp]
        public async Task Setup()
        {
            integrationServer = await IntegrationServer.Start<Program>();
        }

        [Test]
        public async Task Test1()
        {
            await integrationServer.TestApi(async (client) =>
            {
                var response = await client.GetAsync("/api/values");
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            });
        }
    }
}
