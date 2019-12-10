using System;
using lifebook.core.cqrses.Attributes;
using lifebook.core.cqrses.Domains;
using lifebook.core.cqrses.Services;

namespace SchoolBookApp.Aggregates.Student.CommandHandler
{
    [CommandHandlers]
    [EventHandlers(typeof(StudentAggregate))]
    public class Student : AggregateRoot
    {
        [CommandHandlerFor("CreateStudent")]
        public AggregateEvent CreateStudent(CreateStudent createClassroom)
        {
            return new StudentCreated()
            {
                Homeroom = createClassroom.Homeroom,
                Name = createClassroom.Name,
                Grade = createClassroom.Grade
            };
        }
    }

    public class CreateStudent : Command
    {
        public string Name { get; set; }
        public int Grade { get; set; }
        public Guid Homeroom { get; set; }
    }

    public class StudentCreated : AggregateEvent
    {
        public string Name { get; set; }
        public int Grade { get; set; }
        public Guid Homeroom { get; set; }
    }
}
