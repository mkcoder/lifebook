﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Castle.Windsor;
using lifebook.core.cqrses.Attributes;
using lifebook.core.cqrses.Domains;
using lifebook.core.cqrses.Services;
using lifebook.core.eventstore.domain.api;
using lifebook.core.eventstore.domain.models;
using lifebook.core.eventstore.extensions;
using lifebook.core.eventstore.services;
using lifebook.core.logging.interfaces;
using lifebook.core.services.interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace lifebook.core.cqrses.Filters
{
    public sealed class CommandHandler : IAsyncActionFilter, IAsyncResultFilter, IAsyncExceptionFilter
    {
        private readonly IEventReader _eventReader;
        private readonly IEventWriter _eventWriter;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private Guid _aggregateId = Guid.Empty;
        private Guid _correlationId = Guid.Empty;
        private Guid _causationId = Guid.Empty;
        private string _aggregateType = null;

        public CommandHandler(IServiceProvider serviceProvider, IConfiguration configuration, ILogger logger)
        {
            var container = (WindsorContainer)serviceProvider.GetService(typeof(IWindsorContainer));
            if(!container.Kernel.HasComponent(typeof(IEventStoreClient)))
            {
                container.InstallEventStore();
            }
            _eventReader = container.Resolve<IEventReader>();
            _eventWriter = container.Resolve<IEventWriter>();
            _configuration = configuration;
            _logger = logger;
        }

        public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var isCommandHandler = ((ControllerActionDescriptor)context.ActionDescriptor).MethodInfo.GetCustomAttributes(typeof(CommandHandlerFor), true);
            if (isCommandHandler.Any())
            {                
                var command = (Command)context.ActionArguments.Single().Value;
                _logger.Information($"Command read {command}");
                command.IsValid();
                command.CorrelationId = Guid.NewGuid();
                _aggregateType = command.AggregateType;
                _aggregateId = command.AggregateId;
                _correlationId = command.CorrelationId;
                _causationId = command.CommandId;
            }
            return next();
        }

        public Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var isCommandHandler = ((ControllerActionDescriptor)context.ActionDescriptor).MethodInfo.GetCustomAttributes(typeof(CommandHandlerFor), true);
            if (isCommandHandler.Any())
            {   
                var result = ((ObjectResult)context.Result).Value;
                if (result is AggregateEvent @e)
                {                    
                    @e.CommandName = ((CommandHandlerFor)isCommandHandler[0]).Template;
                    @e.EntityId = _aggregateId;
                    @e.CorrelationId = _correlationId;
                    @e.CausationId = _causationId;
                    ((ObjectResult)context.Result).Value = @e;
                    _logger.Information($"Aggregate Event write {((ObjectResult)context.Result).Value}");
                    _eventWriter.WriteEventAsync(new StreamCategorySpecifier(_configuration["ServiceName"], _configuration["ServiceInstance"], _aggregateType, e.EntityId), @e);
                }
                else
                {
                    throw new Exception("Commands must return IEnumerable<IEvent> or IEvent");
                }

            }            
            return next();
        }

        private string GetAggregateFromRequest(PathString path)
        {
            return path.Value.Split('/').First();
        }

        public Task OnExceptionAsync(ExceptionContext context)
        {
            return Task.Run(() => Console.WriteLine(context));
        }
    }
}
