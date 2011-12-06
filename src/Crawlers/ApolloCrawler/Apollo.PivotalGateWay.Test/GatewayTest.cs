using System;
using System.Linq;
using NUnit.Framework;

namespace Apollo.PivotalGateway.Test
{
    [TestFixture]
    public class GatewayTest
    {
        [Test]
        public void GetAllProjects()
        {

            var pivotHub = new Agent("340079");

            var projects = pivotHub.Projects;

            Assert.IsNotNull(projects);

            Console.WriteLine(projects.Where(x => x.Name == "Program Mgmt").First().Current_Velocity);
        }

        [Test]
        public void GetProjectName()
        {
            var pivotHub = new Agent("340079");

            Assert.IsNotNullOrEmpty(pivotHub.Project.Name);

            Console.WriteLine(pivotHub.Project.Name);
        }

        [Test]
        public void GetIterationCount()
        {
            var pivotHub = new Agent("340079");

            Assert.Greater(pivotHub.Iterations.Count(),0);

            Console.WriteLine(pivotHub.Iterations.Count());
            
        }

        [Test]
        public void GetFirstStory()
        {
            var pivotHub = new Agent("340079");
            var firstStory = pivotHub.Stories(100, 1);
            Assert.IsNotNull(firstStory);

            Console.WriteLine(firstStory.First().Description);

        }
    }
}
