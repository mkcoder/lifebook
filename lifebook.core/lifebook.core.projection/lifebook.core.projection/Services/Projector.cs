using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using lifebook.core.projection.Attributes;
using lifebook.core.projection.Domain;
using lifebook.core.projection.Interfaces;

namespace lifebook.core.projection.Services
{
    public abstract class Projector<T> where T: EntityProjection
    {
        protected T Value { get; }

        private readonly IProjectionStore _projectionStore;
        
        public Projector(IProjectionStore projectionStore)
        {
            _projectionStore = projectionStore;
        }

        protected virtual void Start()
        {
            var methods = GetType()
                        .GetMethods()
                        .Where(m => m.GetCustomAttributes(typeof(UponEvent), false).Count() > 0)
                        .Select(m =>
                                    m
                                    .GetCustomAttributes(typeof(UponEvent), false)
                                    .Select(a => ((UponEvent)a).EventName)
                                    .ToDictionary(en => en, mi => m)
                        )
                        .Aggregate(new Dictionary<string, MethodInfo>(), (prev, value) => prev.Union(value).ToDictionary(k => k.Key, v => v.Value));

        }
    }
}
