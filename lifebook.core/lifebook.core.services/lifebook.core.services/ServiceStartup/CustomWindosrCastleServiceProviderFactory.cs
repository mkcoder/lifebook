using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Castle.Windsor.MsDependencyInjection;
using lifebook.core.logging.interfaces;
using lifebook.core.logging.services;
using lifebook.core.services.extensions;
using lifebook.core.services.LifebookContainer;
using Microsoft.Extensions.DependencyInjection;

namespace lifebook.core.services.ServiceStartup
{
    public class CustomWindosrCastleServiceProviderFactory : IServiceProviderFactory<IWindsorContainer>
    {
        private IServiceResolver serviceResolver;

        public CustomWindosrCastleServiceProviderFactory(IServiceResolver serviceResolver)
        {
            this.serviceResolver = serviceResolver;
        }

        public IWindsorContainer CreateBuilder(IServiceCollection services)
        {
            var container = new WindsorContainer();
			container.Register(Component.For<ILifebookContainer>().Instance((LifebookContainerToWindsorContainerProxy)container));
            container.Register(Component.For<IServiceProvider>().ImplementedBy<WindosrCastleServiceProvider>());
			container.AddServices(services);
			container.Install(FromAssembly.InThisApplication(GetType().Assembly.GetRootAssembly()));
			serviceResolver?.ServiceResolver(container.Resolve<ILifebookContainer>());
			return container;
		}

        public IServiceProvider CreateServiceProvider(IWindsorContainer builder)
        {
            return new WindosrCastleServiceProvider(builder, builder.Resolve<MsLifetimeScopeProvider>());
        }
    }

    public class WindosrCastleServiceProvider : IServiceProvider, ISupportRequiredService
    {
        private readonly IWindsorContainer _container;
        protected IMsLifetimeScope OwnMsLifetimeScope { get; }

        public static bool IsInResolving
        {
            get => _isInResolving.Value;
            set => _isInResolving.Value = value;
        }

        private static readonly AsyncLocal<bool> _isInResolving = new AsyncLocal<bool>();

        public WindosrCastleServiceProvider(IWindsorContainer container, MsLifetimeScopeProvider msLifetimeScopeProvider)
        {
            _container = container;
            OwnMsLifetimeScope = msLifetimeScopeProvider.LifetimeScope;
        }

        public object GetService(Type serviceType)
        {
            return GetServiceInternal(serviceType, true);
        }

        public object GetRequiredService(Type serviceType)
        {
            return GetServiceInternal(serviceType, false);
        }

        private object GetServiceInternal(Type serviceType, bool isOptional)
        {
            using (MsLifetimeScope.Using(OwnMsLifetimeScope))
            {
                var isAlreadyInResolving = IsInResolving;

                if (!isAlreadyInResolving)
                {
                    IsInResolving = true;
                }

                object instance = null;
                try
                {
                    return instance = ResolveInstanceOrNull(serviceType, isOptional);
                }
                finally
                {
                    if (!isAlreadyInResolving)
                    {
                        if (instance != null)
                        {
                            OwnMsLifetimeScope?.AddInstance(instance);
                        }

                        IsInResolving = false;
                    }
                }
            }
        }

        private object ResolveInstanceOrNull(Type serviceType, bool isOptional)
        {
            //Check if given service is directly registered
            if (_container.Kernel.HasComponent(serviceType))
            {
                return _container.Resolve(serviceType);
            }

            // Check if requested IEnumerable<TService>
            // MS uses GetService<IEnumerable<TService>>() to get a collection.
            // This must be resolved with IWindsorContainer.ResolveAll();

            if (serviceType.GetTypeInfo().IsGenericType && serviceType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                var allObjects = _container.ResolveAll(serviceType.GenericTypeArguments[0]);
                Array.Reverse(allObjects);
                return allObjects;
            }

            if (isOptional)
            {
                //Not found
                return null;
            }

            //Let Castle Windsor throws exception since the service is not registered!
            return _container.Resolve(serviceType);
        }
    }
}