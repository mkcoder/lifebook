namespace lifebook.core.messagebus.Models
{
    public class MessageQueueInformation
    {
        public string QueueName { get; set; }
        public string ExchangeName { get; set; }
        public string RoutingKey { get; set; }
    }
}
