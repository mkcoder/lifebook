using System;
using Microsoft.AspNetCore.Mvc;

namespace lifebook.core.cqrses.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class CommandHandlerFor : HttpPostAttribute
    {
        public CommandHandlerFor(string commandName) : base(commandName)
        {
        }
    }
}
