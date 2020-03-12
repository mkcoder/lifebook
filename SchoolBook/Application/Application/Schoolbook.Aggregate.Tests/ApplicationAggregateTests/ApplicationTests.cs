using System;
using System.Threading.Tasks;
using lifebook.core.services.Testing;
using Schoolbook.Application;

namespace Schoolbook.Aggregate.Tests.ApplicationAggregateTests
{
    public class ApplicationTests
    {
        IntegrationServer server;

        public async Task Setup()
        {
            server = await IntegrationServer.Start<Program>();
        }

        public async Task CreateApplicationWithCorrectValueWorks()
        {

        }
    }
}
