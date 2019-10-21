using System;
using System.Collections.Generic;
using System.Linq;
using lifebook.core.projection.Attributes;
using lifebook.core.projection.Domain;
using lifebook.core.projection.Interfaces;
using lifebook.core.projection.Util;
using lifebook.core.services.interfaces;
using Microsoft.EntityFrameworkCore;

namespace lifebook.core.projection.Services.StreamTracker
{
    public class EntityStreamTracker : IStreamTracker
    {
        private readonly IApplicationContext _applicationContext;
        private readonly DbSet<StreamTrackingInformation> _streamTrackingInformation;
        private static readonly object _lock = new object();

        public EntityStreamTracker(IApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
            _streamTrackingInformation = _applicationContext.Get<StreamTrackingInformation>();
        }

        public List<StreamTrackingInformation> Track<T>(Projector<T> projector) where T : EntityProjection, new()
        {
            var result = new List<StreamTrackingInformation>();
            lock (_lock)
            {
                var uniqueName = projector.GetType().FullName;
                var streamCategory = projector.GetType().GetCustomAttributes(typeof(StreamCategory), false);
                foreach (StreamCategory category in streamCategory)
                {
                    var sti = new StreamTrackingInformation();
                    sti.StreamId = category.ToString();
                    sti.StreamKey = $"{uniqueName}";
                    sti.Id = StringExtensions.ToGuid($"{uniqueName}-{category.ToString()}");
                    var entry = _applicationContext.Get<StreamTrackingInformation>();
                    var record = entry.Where(s => s.Id == sti.Id).FirstOrDefault();
                    if (record == null)
                    {
                        entry.Add(sti);
                        result.Add(sti);
                    }
                    else
                    {
                        result.Add(record);
                    }
                }
                _applicationContext.TrySaveChangesOrFail();                
            }
            return result;
        }

        public StreamTrackingInformation Update(Guid streamId, long newEventNumber)
        {
            StreamTrackingInformation info;
            lock (_lock)
            {
                info = _streamTrackingInformation.FirstOrDefault(r => r.Id == streamId);
                info.LastEventRead = newEventNumber;
                _streamTrackingInformation.Update(info);
                _applicationContext.TrySaveChangesOrFail();
            }
            return info;
        }

        public long GetLastEventStoredFromStream(string streamKey)
        {
            long? result = null;

            lock (_lock)
            {
                result = _streamTrackingInformation.FirstOrDefault(r => r.StreamKey == streamKey)?.LastEventRead;
            }

            if(result == null) throw new ArgumentNullException(nameof(streamKey), "No entry with this key exist");
            return result.Value;
        }

        public long GetLastEventStoredFromStream(Guid streamId)
        {
            long? result = null;
            lock(_lock)
            {
                result = _streamTrackingInformation.FirstOrDefault(r => r.Id == streamId)?.LastEventRead;
            }
            if (result == null) throw new ArgumentNullException(nameof(streamId), "No entry with this key exist");
            return result.Value;            
        }
    }
}