using System;
using lifebook.core.cqrses.Attributes;
using lifebook.core.cqrses.Domains;
using lifebook.core.cqrses.Services;

namespace SchoolBookApp.Aggregates.Relationship.CommandHandler
{
    [CommandHandlers]
    [EventHandlers(typeof(RelationshipCommandHandlerAggregate))]
    public class Homework : AggregateRoot
    {
        [CommandHandlerFor("CreateHomework")]
        public AggregateEvent CreateHomework(CreateHomeworkCommand createClassroom)
        {
            return new HomeworkCreated()
            {
            };
        }

        public class CreateHomeworkCommand : Command
        {
        }
    }

    internal class HomeworkCreated : AggregateEvent
    {
    }
}
