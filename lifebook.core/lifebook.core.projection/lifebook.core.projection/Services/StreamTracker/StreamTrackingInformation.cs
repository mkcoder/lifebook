using System;

namespace lifebook.core.projection.Services.StreamTracker
{
    public class StreamTrackingInformation
    {
        public Guid Id { get; set; }
        public string StreamKey { get; set; }
        public string StreamId { get; set; }
        public long LastEventRead { get; set; }
    }
}
