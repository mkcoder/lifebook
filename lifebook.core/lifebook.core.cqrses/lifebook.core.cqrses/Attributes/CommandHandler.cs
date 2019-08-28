using System;
using lifebook.core.cqrses.Filters;
using Microsoft.AspNetCore.Mvc;

namespace lifebook.core.cqrses.Attributes
{
    public class Aggregate : TypeFilterAttribute
    {
        public Aggregate() : base(typeof(CommandHandler))
        {
        }
    }
}
