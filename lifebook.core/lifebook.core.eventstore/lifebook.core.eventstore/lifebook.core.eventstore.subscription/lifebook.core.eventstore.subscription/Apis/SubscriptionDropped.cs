using System;
namespace lifebook.core.eventstore.subscription.Apis
{
    public class SubscriptionDropped
    {
        public string SubscriptionName { get; set; }
        public string StreamId { get; internal set; }
        public string Reason { get; internal set; }
        public string Message { get; internal set; }
    }
}
