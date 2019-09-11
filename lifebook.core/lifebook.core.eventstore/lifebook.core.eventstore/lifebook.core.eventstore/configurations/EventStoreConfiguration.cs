using lifebook.core.services.interfaces;

namespace lifebook.core.eventstore.configurations
{
    public class EventStoreConfiguration
    {
        private readonly IConfiguration _configuration;

        public string IpAddress { get; } 
        public int Port { get; }
        public int ReadPerCycle { get; }
        public bool UseFakeEventStore { get; }

        public EventStoreConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;

            IpAddress = _configuration["EventStore.IpAddress"];
            Port = _configuration.GetValue<int>("EventStore.Port");
            ReadPerCycle = _configuration.GetValue<int>("EventStore.ReadPerCycle");
            UseFakeEventStore = _configuration.GetValue<bool>("EventStore.UseFakeEventStore"); 
        }
    }
}
