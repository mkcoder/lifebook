using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using lifebook.core.services.extensions;
using System;
using System.Reflection;

namespace lifebook.core.services.LifebookContainer
{
	public interface ILifebookContainer
	{
		public ILifebookContainer Register<T, Impl>(Lifetime a = Lifetime.Transient) where Impl : T where T : class;
		public T Resolve<T>();
		public IComplexRegisteration ComplexRegisteration();
	}

	public interface IComplexRegisteration
	{
		IComplexRegisteration RegisterAllServicesBasedOnInterface<TBase, T>(Lifetime lifetime = Lifetime.Transient) where T : class;
		IComplexRegisteration RegisterAllServicesBasedOnInterface<T>(Lifetime lifetime = Lifetime.Transient) where T : class;
		IComplexRegisteration RegisterAllServicesBasedOnInterface(Type t, Lifetime lifetime = Lifetime.Transient);
		ILifebookContainer Container();
	}

	public enum Lifetime
	{
		Singleton,
		Transient,
		TransientPerService
	}

	public class WindsorContainerComplexRegisterationBuilder : IComplexRegisteration
	{
		private LifebookContainerToWindsorContainerProxy container;

		public WindsorContainerComplexRegisterationBuilder(LifebookContainerToWindsorContainerProxy container)
		{
			this.container = container;
		}

		public ILifebookContainer Container() => container;

		public IComplexRegisteration RegisterAllServicesBasedOnInterface<TBase, T>(Lifetime lifetime = Lifetime.Transient) where T : class
		{
			BasedOnDescriptor classes = Classes.FromAssemblyInThisApplication(typeof(TBase).Assembly)
					   .BasedOn<T>()
					   .LifestyleTransient()
					   .WithServiceSelf()
					   .WithServiceAllInterfaces();
			RegisterAllServicesBasedOnInterface(classes, lifetime);
			return this;
		}

		public IComplexRegisteration RegisterAllServicesBasedOnInterface<T>(Lifetime lifetime = Lifetime.Transient) where T : class
		{

			var classes = Classes.FromAssemblyInThisApplication(LifebookContainerToWindsorContainerProxy.RootAssembly)
					   .BasedOn<T>()
					   .WithServiceSelf()
					   .WithServiceAllInterfaces();

			RegisterAllServicesBasedOnInterface(classes, lifetime);


			return this;
		}

		public IComplexRegisteration RegisterAllServicesBasedOnInterface(Type t, Lifetime lifetime = Lifetime.Transient)
		{
			BasedOnDescriptor classes = Classes.FromAssemblyInThisApplication(LifebookContainerToWindsorContainerProxy.RootAssembly)
						  .BasedOn(t)
						  .WithServiceSelf()
						  .WithServiceAllInterfaces();
			RegisterAllServicesBasedOnInterface(classes, lifetime);
			return this;
		}

		private void RegisterAllServicesBasedOnInterface(BasedOnDescriptor classes, Lifetime lifetime)
		{
			switch (lifetime)
			{
				case Lifetime.Singleton:
					classes.LifestyleSingleton();
					break;
				default:
					classes.LifestyleTransient();
					break;
			}

			container.container.Register(
				classes
			);
		}
	}

	public class LifebookContainerToWindsorContainerProxy : ILifebookContainer
	{
		internal IWindsorContainer container;
		internal static Assembly RootAssembly;

		public LifebookContainerToWindsorContainerProxy() : this(new WindsorContainer())
		{

		}

		public LifebookContainerToWindsorContainerProxy(WindsorContainer proxy)
		{
			container = proxy;
			RootAssembly = GetType().Assembly.GetRootAssembly();
		}

		public IComplexRegisteration ComplexRegisteration()
		{
			return new WindsorContainerComplexRegisterationBuilder(this);
		}

		public ILifebookContainer Register<T, Impl>(Lifetime a = Lifetime.Transient) where Impl : T where T : class
		{
			switch (a)
			{
				case Lifetime.Singleton:
					container.Register(Component.For<T>().ImplementedBy<Impl>().IsDefault().LifeStyle.Singleton);
					break;
				case Lifetime.Transient:
					container.Register(Component.For<T>().ImplementedBy<Impl>().IsDefault().LifeStyle.Transient);
					break;
				case Lifetime.TransientPerService:
					container.Register(Component.For<T>().ImplementedBy<Impl>().IsDefault().LifeStyle.Transient.OnlyNewServices());
					break;
			}
			return this;
		}

		public static implicit operator WindsorContainer(LifebookContainerToWindsorContainerProxy proxy)
		{
			return (WindsorContainer)proxy.container;
		}

		public static implicit operator LifebookContainerToWindsorContainerProxy(WindsorContainer proxy)
		{
			return new LifebookContainerToWindsorContainerProxy(proxy);
		}

		public T Resolve<T>()
		{
			return container.Resolve<T>();
		}
	}

	public abstract class BaseLifebookModuleInstaller : IWindsorInstaller
	{
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			Install(container.Resolve<ILifebookContainer>());
		}

		public abstract void Install(ILifebookContainer container);
	}
}
