﻿using System;
using lifebook.core.cqrses.Services;

namespace SchoolBookApp.Aggregates.Classroom.Commands
{
    public class CreateGradebook : Command
    {
        public string ClassName { get; set; }
        public string RoomNumber { get; set; }
    }

    public class AssignHomeworkToClass
    {
    }
}
