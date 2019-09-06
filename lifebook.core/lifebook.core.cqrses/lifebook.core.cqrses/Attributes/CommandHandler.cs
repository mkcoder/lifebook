using System;
using lifebook.core.cqrses.Filters;
using Microsoft.AspNetCore.Mvc;

namespace lifebook.core.cqrses.Attributes
{
    public class CommandHandlers : TypeFilterAttribute
    {
        public CommandHandlers() : base(typeof(CommandHandler))
        {
        }
    }
}
