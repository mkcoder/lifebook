using System;
using lifebook.core.cqrses.Services;

namespace SchoolBookApp.Aggregates.Relationship.Commands
{
    public class AssignHomeworkToClassroom : Command
    {
        public Guid HomeworkId { get; set; }
        public Guid ClassroomId { get; set; }
    }
}
