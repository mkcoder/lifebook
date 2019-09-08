using System;
using System.Collections.Generic;
using lifebook.core.services.configuration;
using Microsoft.Extensions.Configuration;

namespace lifebook.core.eventstore.providers
{
    public class DevelopmentEventStoreConfigurationProvider : IConfigurationProviderInistalizer
    {
        public void Provide(IConfigurationBuilder cb)
        {
            List<KeyValuePair<string, string>> defaultConfiguration = new List<KeyValuePair<string, string>>();
            defaultConfiguration.Add(new KeyValuePair<string, string>("EventStore.IpAddress", "127.0.0.1"));
            defaultConfiguration.Add(new KeyValuePair<string, string>("EventStore.Port", "1113"));
            defaultConfiguration.Add(new KeyValuePair<string, string>("EventStore.UseFakeEventStore", "false"));
            cb.AddInMemoryCollection(defaultConfiguration);
        }
    }
}
