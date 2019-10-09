using lifebook.core.projection.Domain;
using lifebook.core.projection.Services;
using NUnit.Framework;

namespace lifebook.core.projection.tests
{
    public class PersonEntity : EntityProjection
    {

    }

    public class PersonProjector : Projector<>
    {

    }


    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}