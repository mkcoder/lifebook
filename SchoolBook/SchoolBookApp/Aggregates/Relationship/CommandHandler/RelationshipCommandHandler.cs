using System;
using lifebook.core.cqrses.Attributes;
using lifebook.core.cqrses.Domains;
using lifebook.core.cqrses.Services;

namespace SchoolBookApp.Aggregates.Relationship.CommandHandler
{
    [CommandHandlers]
    [EventHandlers(typeof(RelationshipCommandHandlerAggregate))]
    public class RelationshipCommandHandler : AggregateRoot
    {
        [CommandHandlerFor("AssignHomeworkToClassroom")]
        public AggregateEvent CreateClassroom(AssignHomeworkToClassroom createClassroom)
        {
            return new HomeworkAssignedToClassroom()
            {
                HomeworkId = createClassroom.HomeworkId,
                ClassroomId = createClassroom.ClassroomId
            };
        }
    }

    public class AssignHomeworkToClassroom
    {
        public Guid HomeworkId { get; set; }
        public Guid ClassroomId { get; set; }
    }
}
