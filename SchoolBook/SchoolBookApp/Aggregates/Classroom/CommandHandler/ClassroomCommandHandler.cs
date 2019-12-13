using System;
using System.Collections.Generic;
using System.Text.Json;
using lifebook.core.cqrses.Attributes;
using lifebook.core.cqrses.Domains;
using lifebook.core.cqrses.Services;
using SchoolBookApp.Aggregates.Classroom.Commands;
using SchoolBookApp.Aggregates.Classroom.Events;

namespace SchoolBookApp.Aggregates.Classroom.CommandHandler
{
    public class ClassroomAggregate : Aggregate
    {
        public string ClassName { get; set; }
        public string RoomNumber { get; set; }

        public ClassroomAggregate GetAggregate() => this;

        [EventHandlerFor("ClassroomCreated")]
        public void PersonCreated(AggregateEvent e)
        {
            var personCreated = e.Data.TransformDataFromString(j => JsonSerializer.Deserialize<ClassroomCreated>(j));
            ClassName = personCreated.ClassName;
            RoomNumber = personCreated.RoomNumber;
        }      
    }

    [CommandHandlers]
    [EventHandlers(typeof(ClassroomAggregate))]
    public class Classroom : AggregateRoot
    {
        [CommandHandlerFor("CreateClassroom")]
        public AggregateEvent CreateClassroom(CreateGradebook createClassroom)
        {
            return new ClassroomCreated()
            {
                ClassName = createClassroom.ClassName,
                RoomNumber = createClassroom.RoomNumber
            };
        }
    }
}
