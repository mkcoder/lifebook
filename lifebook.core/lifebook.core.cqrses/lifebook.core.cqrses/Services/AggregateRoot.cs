using System;
using Microsoft.AspNetCore.Mvc;

namespace lifebook.core.cqrses.Services
{
    public class AggregateRoot : ControllerBase
    {
        private dynamic _aggregate;

        public void SetAggregate(dynamic aggregate)
        {
            _aggregate = aggregate;
        }

        public TOut WithAggregate<T, TOut>(Func<T, TOut> action)
        {
            return action((T)Convert.ChangeType(_aggregate, typeof(T)));
        }
    }
}
