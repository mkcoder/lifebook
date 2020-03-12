using System;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using lifebook.core.services.discovery;
using lifebook.core.services.extensions;
using lifebook.core.services.interfaces;
using lifebook.core.services.LifebookContainer;

namespace lifebook.core.services.Testing
{
    public class IntegrationServer
    {
        private static ILifebookContainer lifebookContainer;
        private static HttpClient client;

        private IntegrationServer() { }

        public static async Task<IntegrationServer> Start<T>()
        {
            Environment.SetEnvironmentVariable(IntegrationService.ENV, "true");
            Thread t = new Thread(_ =>
            {
                extensions.AssemblyExtensions.SetAssemblyRoot(typeof(T).Assembly);
                var m = typeof(T).GetMethod("Main") ?? typeof(T).GetMethod("Main", BindingFlags.NonPublic | BindingFlags.Static);
                m.Invoke(null, new object[] { new string[] { } });
            });
            t.Start();
            while (!IntegrationService.Started) Thread.Sleep(2000);
            Environment.SetEnvironmentVariable(IntegrationService.ENV, null);
            var network = IntegrationService.Container.Resolve<INetworkServiceLocator>();
            var configuration = IntegrationService.Container.Resolve<IConfiguration>();
            var service = await network.FindServiceByName($"{configuration.TryGetValueOrDefault("ServiceInstance", "")}_{configuration.TryGetValueOrDefault("ServiceName", "")}");
            client = new HttpClient()
            {
                BaseAddress = new Uri($"http://{service.FullAddress}"),
                Timeout = TimeSpan.FromMinutes(5)
            };
            return new IntegrationServer();
        }

        public async Task TestApi(Func<HttpClient, ILifebookContainer, Task> api)
        {
            try
            {
                await api(client, IntegrationService.Container);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public async Task TestApi(Func<HttpClient, Task> api)
        {
            try
            {
                await api(client);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

    }
}
