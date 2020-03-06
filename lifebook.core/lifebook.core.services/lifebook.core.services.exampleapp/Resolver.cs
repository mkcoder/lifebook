using lifebook.core.services.interfaces;
using lifebook.core.services.LifebookContainer;
using lifebook.core.services.ServiceStartup;
using Microsoft.Extensions.DependencyInjection;

namespace lifebook.core.services.exampleapp
{
	public class CustomerInstaller : BaseLifebookModuleInstaller
	{
		public override void Install(ILifebookContainer container)
		{
			container.Register<IGetPerson, GetPerson>();
		}
	}

	public class GetJohn : IGetPerson
	{
		public string Name()
		{
			return "John";
		}
	}
}