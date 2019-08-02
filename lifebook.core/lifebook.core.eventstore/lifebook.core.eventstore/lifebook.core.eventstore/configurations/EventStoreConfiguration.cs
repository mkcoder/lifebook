using System;
using Microsoft.Extensions.Configuration;

namespace lifebook.core.eventstore.configurations
{
    public class EventStoreConfiguration
    {
        private readonly IConfigurationRoot configuration;

        public string IpAddress { get; } 
        public int Port { get; }
        public bool UseFakeEventStore { get; }

        public EventStoreConfiguration(IConfigurationBuilder configurationBuilder)
        {
            configuration = configurationBuilder
                                .AddJsonFile("eventstoreconfiguration.dev.json")
                                .Build();

            IpAddress = configuration["EventStore.IpAddress"];
            Port = int.Parse(configuration["EventStore.Port"]);
            UseFakeEventStore = bool.Parse(configuration["EventStore.UseFakeEventStore"]);
        }
    }
}
