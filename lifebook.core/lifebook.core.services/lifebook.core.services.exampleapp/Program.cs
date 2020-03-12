using System;
using Castle.Windsor;
using lifebook.core.services.LifebookContainer;
using lifebook.core.services.ServiceStartup;

namespace lifebook.core.services.exampleapp
{
    public class Program
    {
        static void Main(string[] args)
		{
			Hosting.Start<BaseServiceStartup>(new ResolveAfterConfigurationSetup());
		}
	}
}
