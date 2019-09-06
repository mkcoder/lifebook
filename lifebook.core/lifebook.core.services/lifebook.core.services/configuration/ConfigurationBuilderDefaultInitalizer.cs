using System.Collections;
using System.Collections.Generic;
using System.Linq;
using lifebook.core.services.attribute;
using Microsoft.Extensions.Configuration;

namespace lifebook.core.services.configuration
{
    public class ConfigurationBuilderDefaultInitalizer
    {
        private readonly IList<IConfigurationProviderInistalizer> _configuration;

        public ConfigurationBuilderDefaultInitalizer(IConfigurationProviderInistalizer[] configuration)
        {
            // Order:
            // Development
            // Production        
            _configuration = configuration
                .OrderBy(c => c.GetType().GetCustomAttributes(typeof(ProductionConfigurationAttribute), false))
                .ToList();
        }

        public void configure(IConfigurationBuilder arg)
        {
            foreach (var provider in _configuration)
            {

                provider.Provide(arg);
            }
        }
    }
}