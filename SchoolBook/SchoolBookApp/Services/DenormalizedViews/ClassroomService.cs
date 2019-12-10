using System;
using System.Collections.Generic;
using lifebook.SchoolBookApp.Projectors;

namespace lifebook.SchoolBookApp.Services.DenormalizedViews
{
    public class ClassroomService
    {
        private readonly ClassroomProjector _classroomProjector;
        //private readonly AssignmentToClassroomProjector _assignmentToClassroomProjector;

        public ClassroomService(ClassroomProjector classroomProjector)
        {
            _classroomProjector = classroomProjector;
            //_assignmentToClassroomProjector = assignmentToClassroomProjector;
        }

        public List<Assignment> GetAllAssignmentsForClassroom(Guid classroomId)
        {
            return null;
        }
    }

    public class Assignment
    {
    }
}
