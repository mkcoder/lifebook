using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using lifebook.core.processmanager.Domain;
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
                        .ImplementedBy<ProcessManagerServices>(),
                    Component
                        .For<IDomainEventManager>()
                        .ImplementedBy<DomainEventManager>()
            );


            RegisterMediator(container, store);
        }

        private void RegisterMediator(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromAssembly(Assembly.GetCallingAssembly())
                .BasedOn(typeof(IRequestHandler<,>)).WithServiceAllInterfaces());
            container.Register(Classes.FromAssembly(Assembly.GetCallingAssembly())
                .BasedOn(typeof(INotificationHandler<>)).WithServiceAllInterfaces());

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

    public class ConstrainedRequestPostProcessor<TRequest, TResponse>
        : IRequestPostProcessor<TRequest, TResponse>        
    {
        private readonly TextWriter _writer;

        public ConstrainedRequestPostProcessor(TextWriter writer)
        {
            _writer = writer;
        }

        public Task Process(TRequest request, TResponse response, CancellationToken cancellationToken)
        {
            return _writer.WriteLineAsync("- All Done with Ping");
        }
    }

    public class ContravariantFilter : IHandlersFilter
    {
        public bool HasOpinionAbout(Type service)
        {
            if (!service.IsGenericType)
                return false;

            var genericType = service.GetGenericTypeDefinition();
            var genericArguments = genericType.GetGenericArguments();
            return genericArguments.Count() == 1
                   && genericArguments.Single().GenericParameterAttributes.HasFlag(GenericParameterAttributes.Contravariant);
        }

        public IHandler[] SelectHandlers(Type service, IHandler[] handlers)
        {
            return handlers;
        }
    }

    public class GenericHandler : INotificationHandler<INotification>
    {
        private readonly TextWriter _writer;

        public GenericHandler(TextWriter writer)
        {
            _writer = writer;
        }

        public Task Handle(INotification notification, CancellationToken cancellationToken)
        {
            return _writer.WriteLineAsync("Got notified.");
        }
    }

    public class GenericPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly TextWriter _writer;

        public GenericPipelineBehavior(TextWriter writer)
        {
            _writer = writer;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            await _writer.WriteLineAsync("-- Handling Request");
            var response = await next();
            await _writer.WriteLineAsync("-- Finished Request");
            return response;
        }
    }

    public class GenericRequestPostProcessor<TRequest, TResponse> : IRequestPostProcessor<TRequest, TResponse>
    {
        private readonly TextWriter _writer;

        public GenericRequestPostProcessor(TextWriter writer)
        {
            _writer = writer;
        }

        public Task Process(TRequest request, TResponse response, CancellationToken cancellationToken)
        {
            return _writer.WriteLineAsync("- All Done");
        }
    }

    public class GenericRequestPreProcessor<TRequest> : IRequestPreProcessor<TRequest>
    {
        private readonly TextWriter _writer;

        public GenericRequestPreProcessor(TextWriter writer)
        {
            _writer = writer;
        }

        public Task Process(TRequest request, CancellationToken cancellationToken)
        {
            return _writer.WriteLineAsync("- Starting Up");
        }
    }
}
