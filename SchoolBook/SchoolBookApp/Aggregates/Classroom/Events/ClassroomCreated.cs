using lifebook.core.cqrses.Domains;

namespace SchoolBookApp.Aggregates.Classroom.Events
{
    public class ClassroomCreated : AggregateEvent
    {
        public string ClassName { get; set; }
        public string RoomNumber { get; set; }
    }



    public class HomeworkAssignedToClass : AggregateEvent
    {
    }
}