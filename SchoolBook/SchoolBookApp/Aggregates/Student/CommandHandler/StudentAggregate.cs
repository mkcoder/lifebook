using System;
using System.Text.Json;
using lifebook.core.cqrses.Attributes;
using lifebook.core.cqrses.Domains;
using lifebook.core.cqrses.Services;

namespace SchoolBookApp.Aggregates.Student.CommandHandler
{
    public class StudentAggregate : Aggregate
    {
        public string Name { get; set; }
        public int Grade { get; set; }
        public Guid Homeroom { get; set; }

        public StudentAggregate GetAggregate() => this;

        [EventHandlerFor("StudentCreated")]
        public void StudentCreated(AggregateEvent e)
        {
            var studentCreated = e.Data.TransformDataFromString(j => JsonSerializer.Deserialize<StudentCreated>(j));
            Homeroom = studentCreated.Homeroom;
            Name = studentCreated.Name;
            Grade = studentCreated.Grade;            
        }
    }
}