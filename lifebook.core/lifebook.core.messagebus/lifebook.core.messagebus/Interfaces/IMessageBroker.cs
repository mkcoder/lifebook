using lifebook.core.messagebus.Models;

namespace lifebook.core.messagebus.Interfaces
{
    public interface IMessageBroker
    {
        void Connect();
        IMessageBus TryConnectingDirectlyToQueue(MessageQueueInformation brokerName);
    }
}
