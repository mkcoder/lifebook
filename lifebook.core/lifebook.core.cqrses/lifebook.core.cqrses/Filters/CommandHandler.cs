using System;
using System.Linq;
using System.Threading.Tasks;
using lifebook.core.cqrses.Attributes;
using lifebook.core.cqrses.Domains;
using lifebook.core.cqrses.Services;
using lifebook.core.eventstore.domain.api;
using lifebook.core.eventstore.domain.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace lifebook.core.cqrses.Filters
{
    public sealed class CommandHandler : IAsyncActionFilter, IAsyncResultFilter, IAsyncExceptionFilter
    {
        private readonly IEventReader _eventReader;
        private readonly IEventWriter _eventWriter;

        public CommandHandler(IEventReader eventReader, IEventWriter eventWriter)
        {
            _eventReader = eventReader;
            _eventWriter = eventWriter;
        }

        public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var isCommandHandler = ((ControllerActionDescriptor)context.ActionDescriptor).MethodInfo.GetCustomAttributes(typeof(CommandHandlerFor), true);
            if (isCommandHandler.Any())
            {
                Task.Run(() => Console.WriteLine(context));
                ((ICommand)context.ActionArguments.Single().Value).IsValid();
            }
            return next();
        }

        public Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var isCommandHandler = ((ControllerActionDescriptor)context.ActionDescriptor).MethodInfo.GetCustomAttributes(typeof(CommandHandlerFor), true);
            if (isCommandHandler.Any())
            {   
                var result = ((ObjectResult)context.Result).Value;
                if (result is Event @e)
                {
                    ((ObjectResult)context.Result).Value = @e;
                }
                else
                {
                    throw new Exception("Commands must return IEnumerable<IEvent> or IEvent");
                }

            }            
            return next();
        }

        public Task OnExceptionAsync(ExceptionContext context)
        {
            return Task.Run(() => Console.WriteLine(context));
        }
    }
}
