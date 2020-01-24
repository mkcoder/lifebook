using System;
using lifebook.core.messagebus.Models;

namespace lifebook.core.messagebus.Interfaces
{
    public interface IMessageBus
    {
        MessageConfirmation Publish(object message);
        public void Subscribe<T>(MessageQueueInformation broker, Action<EventBusMessage<T>> action);
    }
}
