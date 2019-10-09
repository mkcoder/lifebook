using System;
using lifebook.core.projection.Interfaces;

namespace lifebook.core.projection.Services
{
    public abstract class Projector<T>
    {
        protected T Value { get; }

        private readonly IProjectionStore _projectionStore;
        
        public Projector(IProjectionStore projectionStore)
        {
            _projectionStore = projectionStore;
        }

        protected virtual void Start()
        {

        }
    }
}
