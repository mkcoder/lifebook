using lifebook.core.cqrses.Services;

namespace lifebook.core.cqrs.tests.Aggregates
{
    public class ChangePersonNameAndAgeCommand : Command
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
