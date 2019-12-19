using System;
using Castle.Windsor;
using lifebook.core.cqrses;
using lifebook.core.orchestrator.Runner;
using lifebook.core.projection.Hosting;
using lifebook.core.services.ServiceStartup;

namespace lifebook.SchoolBookApp
{
    public class Server
    {
        public static void Main(string[] args)
        {
            Hosting.Start<Startup>(new CQRSServiceResolver());
        }
    }

    public class Startup : CQRSStartup
    {
        public override void ServiceStarted(IWindsorContainer windsorContainer)
        {
            base.ServiceStarted(windsorContainer);
            try
            {
                OrchestratorRunner.Run(windsorContainer);
                ProjectorHosting.Run(windsorContainer);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
