using System;
using System.Collections.Generic;
using System.Linq;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using lifebook.core.processmanager.Services;
using MediatR;
using MediatR.Pipeline;

namespace lifebook.core.processmanager.Resolver
{
    public class ResolverBuilder : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                    Component
                        .For<ProcessManagerServices>()
                        .ImplementedBy<ProcessManagerServices>()                   
            );


            RegisterMediator(container, store);
        }

        private void RegisterMediator(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IMediator>().ImplementedBy<Mediator>());
            container.Register(Component.For<ServiceFactory>().UsingFactoryMethod<ServiceFactory>(k => (type =>
            {
                var enumerableType = type
                    .GetInterfaces()
                    .Concat(new[] { type })
                    .FirstOrDefault(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>));

                return enumerableType != null ? k.ResolveAll(enumerableType.GetGenericArguments()[0]) : k.Resolve(type);
            })));
        }
    }
}
