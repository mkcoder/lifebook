using lifebook.core.cqrses.Services;

namespace SchoolBookApp.Aggregates.Gradebook.Commands
{
    public class CreateGradebook : Command
    {
        public string ClassName { get; set; }
        public string RoomNumber { get; set; }
    }
}
