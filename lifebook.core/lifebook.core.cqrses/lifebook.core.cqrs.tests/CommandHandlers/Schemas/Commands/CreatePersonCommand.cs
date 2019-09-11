using lifebook.core.cqrses.Services;

namespace lifebook.core.cqrs.tests.Aggregates
{
    public class CreatePersonCommand : Command
    {
        public string FirstName { get; set; }
        public int Age { get; set; }
    }
}
