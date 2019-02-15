using System;
using System.Threading.Tasks;
using lifebook.core.services.discovery;
using lifebook.core.services.models;
using Xunit;

namespace lifebook.core.services.test
{
    public class UnitTest1
    {
        NetworkServiceLocator locator = new NetworkServiceLocator();
        [Fact]
        public async Task Test1()
        {
            var s = new ServiceInfo();
            s.ServiceName = "Hello world";
            s.Address = "hello";
            s.Port = "Word";
            await Task.Run(() => 1);
            //await locator.RegisterService();
        }
    }
}
