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
    public class EntityStreamTracker : DbContext, IStreamTracker
    {
        public DbSet<StreamTrackingInformation> StreamTrackingInformation { get; set; }
        private readonly IConfiguration _configuration;

        public EntityStreamTracker(DbContextOptions<EntityStreamTracker> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseNpgsql(_configuration.GetValue("ProjectionConnectionString"));
        }

        public List<StreamTrackingInformation> Track<T>(Projector<T> projector) where T : EntityProjection, new()
        {
            var result = new List<StreamTrackingInformation>();
            var uniqueName = projector.GetType().Assembly.FullName;
            var streamCategory = projector.GetType().GetCustomAttributes(typeof(StreamCategory), false);
            foreach (StreamCategory category in streamCategory)
            {
                var sti = new StreamTrackingInformation();
                sti.StreamId = category.ToString();
                sti.StreamKey = $"{category}-{uniqueName}";
                sti.Id = StringExtensions.ToGuid(sti.StreamKey);
                var record = StreamTrackingInformation.FirstOrDefault(s => s.Id == sti.Id);
                if(record == null)
                {
                    StreamTrackingInformation.Add(sti);
                    result.Add(sti);
                }
                else
                {
                    result.Add(record);
                }
            }
            SaveChanges();
            return result;
        }

        public StreamTrackingInformation Update(Guid streamId, long newEventNumber)
        {
            var info = StreamTrackingInformation.FirstOrDefault(r => r.Id == streamId);
            info.LastEventRead = newEventNumber;
            StreamTrackingInformation.Update(info);
            SaveChanges();
            return info;
        }

        public long GetLastEventStoredFromStream(string streamKey)
        {
            return StreamTrackingInformation.FirstOrDefault(r => r.StreamKey == streamKey)?.LastEventRead ?? throw new ArgumentNullException(nameof(streamKey), "No entry with this key exist");
        }

        public long GetLastEventStoredFromStream(Guid streamId)
        {
            return StreamTrackingInformation.FirstOrDefault(r => r.Id == streamId)?.LastEventRead ?? throw new ArgumentNullException(nameof(streamId), "No entry with this key exist");
        }
    }
}