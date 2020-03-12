using System;
namespace lifebook.core.projection.Domain
{
    public class EntityProjection
    {
        public Guid Key { get; set; }
        public DateTime LastUpdated { get; set; }
        public ProjectionStatus Status { get; set; }
    }

    public enum ProjectionStatus
    {
        Running,
        Halted
    }
}
