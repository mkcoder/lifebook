using System;
using lifebook.core.services.configuration;
using Microsoft.Extensions.Configuration;

namespace lifebook.core.examples.mvc
{
    public class EmptyClass : IConfigurationProviderInistalizer
    {
        public EmptyClass()
        {
        }

        public void Provide(IConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.AddInMemoryCollection();
        }
    }
}
