using System.Linq;
using lifebook.core.eventstore.domain.models;
using lifebook.core.processmanager.Syntax;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace lifebook.core.processmanager.tests.SyntaxTests
{
    public class BasicSyntaxTests
    {
        [Test]
        public void ProcessManager_Can_Be_Configured_To_Only_Have_No_Step()
        {
            var configuration = ProcessManagerConfigurationBuilder
                        .Instance
                        .Configuration;
            Assert.AreEqual(0, configuration.GetProcessManagerSteps.Count);
        }

        [Test]
        public void ProcessManager_Can_Be_Configured_To_Only_Have_One_Step()
        {
            string description = "This is a process manager test with 1 step";
            var configuration = ProcessManagerConfigurationBuilder
                        .Instance
                        .UponEvent(new EventSpecifier("ExampleEvent", new StreamCategorySpecifier("TestService", "Test", "ProcessManagerSyntaxTest")))
                        .SetStepDescription(description)
                        .TakeAction(async (e) => { })
                        .Configuration;
            Assert.AreEqual(1, configuration.GetProcessManagerSteps.Count);
            Assert.AreEqual(description, configuration.GetProcessManagerSteps.First().StepDescription);
        }

        [Test]
        public void ProcessManager_Can_Be_Configured_To_Only_Have_Many_Step()
        {            
            var configuration = ProcessManagerConfigurationBuilder
                        .Instance
                        .UponEvent(new EventSpecifier("ExampleEvent 1", new StreamCategorySpecifier("TestService", "Test", "ProcessManagerSyntaxTest")))
                        .SetStepDescription("This is a process manager test. Step 1.")
                        .TakeAction(async (e) => { })
                        .AndThen()
                        .UponEvent(new EventSpecifier("ExampleEvent 2", new StreamCategorySpecifier("TestService", "Test", "ProcessManagerSyntaxTest")))
                        .SetStepDescription("This is a process manager test. Step 2.")
                        .TakeAction(async (e) => { })
                        .AndThen()
                        .UponEvent(new EventSpecifier("ExampleEvent 2", new StreamCategorySpecifier("TestService", "Test", "ProcessManagerSyntaxTest")))
                        .SetStepDescription("This is a process manager test. Step 3.")
                        .TakeAction(async (e) => { })
                        .Configuration;
            Assert.AreEqual(3, configuration.GetProcessManagerSteps.Count);
        }

        [Test]
        public void BasicTest()
        {
            dynamic viewbag = new JObject();
            viewbag.Quote = JObject.FromObject(new object());
            System.Console.WriteLine(viewbag.ToString());
        }
    }
}
