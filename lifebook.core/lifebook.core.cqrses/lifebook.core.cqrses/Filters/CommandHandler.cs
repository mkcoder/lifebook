using System;
using System.Linq;
using System.Threading.Tasks;
using lifebook.core.cqrses.Attributes;
using lifebook.core.cqrses.Domains;
using lifebook.core.cqrses.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace lifebook.core.cqrses.Filters
{
    public sealed class CommandHandler : IAsyncActionFilter, IAsyncResultFilter, IAsyncExceptionFilter
    {
        public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var isCommandHandler = ((ControllerActionDescriptor)context.ActionDescriptor).MethodInfo.GetCustomAttributes(typeof(CommandHandlerFor), true);
            if (isCommandHandler.Any())
            {
                Task.Run(() => Console.WriteLine(context));
                ((ICommand)context.ActionArguments.Single().Value).IsValid();
            }
            return Task.Run(() => next());
        }

        public Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var isCommandHandler = ((ControllerActionDescriptor)context.ActionDescriptor).MethodInfo.GetCustomAttributes(typeof(CommandHandlerFor), true);
            if (isCommandHandler.Any())
            {   
                var result = ((ObjectResult)context.Result).Value;
                if (result is AggregateEvent @e)
                {
                    ((ObjectResult)context.Result).Value = @e;
                }
                else
                {
                    throw new Exception("Commands must return IEnumerable<IEvent> or IEvent");
                }

            }
            return Task.Run(() => next());
        }

        public Task OnExceptionAsync(ExceptionContext context)
        {
            return Task.Run(() => Console.WriteLine(context));
        }
    }
}
