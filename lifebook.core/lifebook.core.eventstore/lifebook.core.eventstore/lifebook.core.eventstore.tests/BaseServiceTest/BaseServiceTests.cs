using System;
using Castle.Windsor;
using NUnit.Framework;

namespace lifebook.core.eventstore.testing.framework
{
    public class BaseServiceTests<T> where T : IInstaller, new()
    {
        protected WindsorContainer container = new WindsorContainer();

        public BaseServiceTests()
        {
            new T().Install(container);
        }
    }
}