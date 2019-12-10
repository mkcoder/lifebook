using System;
using System.Text.Json;
using lifebook.core.cqrses.Domains;
using lifebook.core.projection.Attributes;
using lifebook.core.projection.Domain;
using lifebook.core.projection.Services;
using SchoolBookApp.Aggregates.Classroom.Events;
using SchoolBookApp.Aggregates.Relationship.Events;
using SchoolBookApp.Aggregates.Student.CommandHandler;

namespace lifebook.SchoolBookApp.Projectors
{
    public class Classroom : EntityProjection
    {
        public string ClassName { get; set; }
        public string RoomNumber { get; set; }        
    }

    [StreamCategory("Classroom", "lifebookSchoolbookapp", "Primary")]
    public class ClassroomProjector : Projector<Classroom>
    {
        public ClassroomProjector(ProjectorServices projectorServices) : base(projectorServices)
        {
        }

        [UponEvent("ClassroomCreated")]
        public void PersonCreated(AggregateEvent e)
        {
            var personCreated = e.Data.TransformDataFromString(j => JsonSerializer.Deserialize<ClassroomCreated>(j));
            Value.ClassName = personCreated.ClassName;
            Value.RoomNumber = personCreated.RoomNumber;
        }
    }

    /*public class AssignmentToClassroom : EntityProjection
    {
        public Guid ClassroomId { get; internal set; }
        public Guid HomeworkId { get; internal set; }
    }

    [StreamCategory("RelationshipCommandHandler", "lifebookSchoolbookapp", "Primary")]
    public class AssignmentToClassroomProjector : Projector<AssignmentToClassroom>
    {
        public AssignmentToClassroomProjector(ProjectorServices projectorServices) : base(projectorServices)
        {
        }

        [UponEvent("HomeworkAssignedToClassroom")]
        public void HomeworkAssignedToClassroom(AggregateEvent e)
        {
            var personCreated = e.Data.TransformDataFromString(j => JsonSerializer.Deserialize<HomeworkAssignedToClassroom>(j));
            Value.ClassroomId = personCreated.ClassroomId;
            Value.HomeworkId = personCreated.HomeworkId;
        }
    }

    public class Assignment : EntityProjection
    {
    }

    [StreamCategory("Homework", "lifebookSchoolbookapp", "Primary")]
    public class AssignmentProjector : Projector<Assignment>
    {
        public AssignmentProjector(ProjectorServices projectorServices) : base(projectorServices)
        {
        }

        [UponEvent("HomeworkCreated")]
        public void HomeworkAssignedToClassroom(AggregateEvent e)
        {
        }
    }*/

    public class Student : EntityProjection
    {
        public string Name { get; set; }
        public int Grade { get; set; }
        public Guid Homeroom { get; set; }
    }

    [StreamCategory("Student", "lifebookSchoolbookapp", "Primary")]
    public class StudentProjector : Projector<Student>
    {
        public StudentProjector(ProjectorServices projectorServices) : base(projectorServices)
        {
        }

        [UponEvent("StudentCreated")]
        public void HomeworkAssignedToClassroom(AggregateEvent e)
        {
            var studentCreated = e.Data.TransformDataFromString(j => JsonSerializer.Deserialize<StudentCreated>(j));
            Value.Homeroom = studentCreated.Homeroom;
            Value.Name = studentCreated.Name;
            Value.Grade = studentCreated.Grade;
        }
    }
}
