using System;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using lifebook.core.database.repository.interfaces;
using lifebook.core.database.repository.repositories;
using Xunit;

namespace lifebook.core.database.repository.test
{
    public class UserRepositoryIocTest : TestResolverBase
    {
        public UserRepositoryIocTest() : base()
        {
        }

        protected override void Setup(IWindsorContainer container)
        {
            container.Install(
                FromAssembly.Named("lifebook.core.database.repository")
            );
        }

        public async Task Test1()
        {
            await ExecWithContext((container) =>
             {
                 var result = container.Resolve<IRepository<User>>();
                 var result2 = container.Resolve<UserIdentityDbContext>();
                 var usrs = result.GetAll();
             });
        }
    }
}
