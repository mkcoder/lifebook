using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Windsor;
using lifebook.core.cqrses.Attributes;
using lifebook.core.cqrses.Domains;
using lifebook.core.cqrses.Extensions;
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

        public CommandHandler(IConfiguration configuration, ILogger logger, IEventReader eventReader, IEventWriter eventWriter)
        {
            _eventReader = eventReader;
            _eventWriter = eventWriter;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var isCommandHandler = ((ControllerActionDescriptor)context.ActionDescriptor).MethodInfo.GetCustomAttributes(typeof(CommandHandlerFor), true);
            if (isCommandHandler.Any())
            {                
                var command = (Command)context.ActionArguments.First().Value;

                var mightBeAggregate = (EventHandlersAttribute)context.Controller.GetType().GetCustomAttributes(typeof(EventHandlersAttribute), false).Single();
                await command.IsValid(_eventReader, new StreamCategorySpecifier(_configuration["ServiceName"], _configuration["ServiceInstance"], command.AggregateType, command.AggregateId));
                _aggregateType = command.AggregateType;
                _aggregateId = command.AggregateId;
                _correlationId = command.CorrelationId;
                _causationId = command.CommandId;
                _logger.LogCommand(command);
                var ae = new AggregateReader(_eventReader, _configuration, command, mightBeAggregate);
                var aggregate = await ae.GetAggregate();
                _logger.LogJson("Aggregate hydrated as", aggregate);
                ((AggregateRoot)context.Controller).SetAggregate(aggregate);
            }
            await next();
        }

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var isCommandHandler = ((ControllerActionDescriptor)context.ActionDescriptor).MethodInfo.GetCustomAttributes(typeof(CommandHandlerFor), true);
            if (isCommandHandler.Any())
            {   
                var result = ((ObjectResult)context.Result).Value;
                var commandName = ((CommandHandlerFor)isCommandHandler[0]).Template;
                if (result is AggregateEvent @e)
                {
                    SetupDefaultValues(e, commandName);
                    ((ObjectResult)context.Result).Value = @e;
                    _logger.LogEvent(@e);
                    await _eventWriter.WriteEventAsync(new StreamCategorySpecifier(_configuration["ServiceName"], _configuration["ServiceInstance"], _aggregateType, e.EntityId), @e);
                }
                else if (result is List<AggregateEvent> listOfEvents)
                {
                    foreach (var @event in listOfEvents)
                    {
                        SetupDefaultValues(@event, commandName);
                        _logger.LogEvent(@event);
                        await _eventWriter.WriteEventAsync(new StreamCategorySpecifier(_configuration["ServiceName"], _configuration["ServiceInstance"], _aggregateType, @event.EntityId), @event).ConfigureAwait(false);
                    }
                    ((ObjectResult)context.Result).Value = listOfEvents;
                }
                else
                {
                    throw new Exception("Commands must return IEnumerable<IEvent> or IEvent");
                }

            }            
            await next();
        }

        public AggregateEvent SetupDefaultValues(AggregateEvent aggregateEvent, String commandName)
        {
            aggregateEvent.CommandName = commandName;
            aggregateEvent.EntityId = _aggregateId;
            aggregateEvent.CorrelationId = _correlationId;
            aggregateEvent.CausationId = _causationId;
            return aggregateEvent;
        }

        public async Task OnExceptionAsync(ExceptionContext context)
        {
            _logger.Error(context.Exception, "Error occured trying to handle command");
            
        }
    }
}
