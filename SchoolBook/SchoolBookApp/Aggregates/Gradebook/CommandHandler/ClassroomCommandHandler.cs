using lifebook.core.cqrses.Attributes;
using lifebook.core.cqrses.Domains;
using lifebook.core.cqrses.Services;
using SchoolBookApp.Aggregates.Classroom.Commands;
using SchoolBookApp.Aggregates.Gradebook.Events;

namespace SchoolBookApp.Aggregates.Gradebook.CommandHandler
{
    public class GradebookAggregate : Aggregate
    {
        public GradebookAggregate GetAggregate() => this;

        [EventHandlerFor("GradebookCreated")]
        public void PersonCreated(AggregateEvent e)
        {          
        }      
    }

    [CommandHandlers]
    [EventHandlers(typeof(GradebookAggregate))]
    public class Gradebook : AggregateRoot
    {
        [CommandHandlerFor("CreateGradebook")]
        public AggregateEvent CreateClassroom(CreateGradebook createClassroom)
        {
            return new GradebookCreated()
            {
            };
        }
    }
}
