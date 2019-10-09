using System;
namespace lifebook.core.eventstore.subscription.Apis
{
    public class SubscriptionEvent<T>
    {
        public long EventNumber { get; set; }
        public string StreamInfo { get; set; }
        public T Event { get; set; }
    }
}
