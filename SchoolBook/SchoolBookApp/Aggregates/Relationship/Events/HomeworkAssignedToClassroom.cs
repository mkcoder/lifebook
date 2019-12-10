using System;
using lifebook.core.cqrses.Domains;

namespace SchoolBookApp.Aggregates.Relationship.Events
{
    internal class HomeworkAssignedToClassroom : AggregateEvent
    {
        public Guid HomeworkId { get; internal set; }
        public Guid ClassroomId { get; internal set; }
    }
}