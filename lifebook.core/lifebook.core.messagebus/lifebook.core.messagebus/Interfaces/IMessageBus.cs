using lifebook.core.messagebus.Models;

namespace lifebook.core.messagebus.Interfaces
{
    public interface IMessageBus
    {
        MessageConfirmation Publish(object message);
        T Subscribe<T>(MessageQueueInformation broker);
    }
}
