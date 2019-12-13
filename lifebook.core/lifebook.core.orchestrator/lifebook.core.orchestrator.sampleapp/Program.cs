using System;
using System.Threading;
using System.Threading.Tasks;
using Castle.Windsor;
using Castle.Windsor.Installer;
using lifebook.core.logging.ioc;

namespace lifebook.core.orchestrator.sampleapp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var container = new WindsorContainer();
            container.Install(FromAssembly.InThisApplication(typeof(BootLoader).Assembly));
            await Runner.OrchestratorRunner.Run(container);

            while (true)
            {
                Thread.Sleep(1000);
            }
        }
    }
}
