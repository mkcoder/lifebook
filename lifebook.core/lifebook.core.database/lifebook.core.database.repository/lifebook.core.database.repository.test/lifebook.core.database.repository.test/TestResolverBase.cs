using System;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Castle.Windsor.Installer;

namespace lifebook.core.database.repository.test
{
    public abstract class TestResolverBase
    {
        private IWindsorContainer _container;

        public TestResolverBase()
        {
            _container = new WindsorContainer();
            _container.Install();
            Setup(_container);
        }

        protected abstract void Setup(IWindsorContainer container);

        public Task ExecWithContext(Action<IWindsorContainer> action)
        {
            return Task.Run(() => action(_container));        
        }
    }
}
