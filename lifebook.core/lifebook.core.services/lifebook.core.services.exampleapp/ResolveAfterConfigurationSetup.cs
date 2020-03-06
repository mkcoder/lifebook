using System.Threading.Tasks;
using lifebook.core.services.interfaces;
using lifebook.core.services.LifebookContainer;
using lifebook.core.services.ServiceStartup;
using Microsoft.Extensions.DependencyInjection;

namespace lifebook.core.services.exampleapp
{
	internal class ResolveAfterConfigurationSetup : IServiceResolver
	{
		public void ServiceResolver(ILifebookContainer container)
		{
			var t = container.Resolve<IConfiguration>();
			if (t.TryGetValueOrDefault("IsProduction", false))
			{
				container.Register<IGetPerson, GetJohn>();
			}
		}
	}
}