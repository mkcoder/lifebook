using System;
using Castle.Windsor;

namespace lifebook.core.eventstore.sampleapp
{
    public interface IExample
    {
        void Run(string[] args, WindsorContainer container);
    }
}
