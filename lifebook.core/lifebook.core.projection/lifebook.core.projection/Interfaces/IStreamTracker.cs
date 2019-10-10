using System;
using System.Collections.Generic;
using lifebook.core.projection.Attributes;
using lifebook.core.projection.Domain;
using lifebook.core.projection.Services;
using lifebook.core.projection.Services.StreamTracker;

namespace lifebook.core.projection.Interfaces
{
    public interface IStreamTracker
    {
        List<StreamTrackingInformation> Track<T>(Projector<T> projector) where T : EntityProjection;
        long GetLastEventStoredFromStream(string streamKey);
        long GetLastEventStoredFromStream(Guid streamId);
        StreamTrackingInformation Update(Guid streamId, long newEventNumber);
    }
}
