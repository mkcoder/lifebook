using System;
using Microsoft.AspNetCore.Mvc;

namespace lifebook.core.cqrses.Services
{
    [ApiController]
    public class AggregateRoot : ControllerBase
    {
        private dynamic _aggregate;

        internal void SetAggregate(dynamic aggregate)
        {
            _aggregate = aggregate;
        }

        protected TOut WithAggregate<T, TOut>(Func<T, TOut> action)
        {
            return action((T)Convert.ChangeType(_aggregate, typeof(T)));
        }
    }
}
