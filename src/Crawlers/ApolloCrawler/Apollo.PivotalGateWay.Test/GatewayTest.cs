using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Apollo.PivotalGateway.Test
{
    [TestFixture]
    public class GatewayTest
    {
        [Test]
        public void GetProjectName()
        {
            int projectId = 340079;

            var pivotHub = new Apollo.PivotalGateway.Harvester(340079);

            Assert.IsNotNullOrEmpty(pivotHub.Project.Name);

            Console.WriteLine(pivotHub.Project.Name);
        }

        [Test]
        public void GetIterationCount()
        {
            int projectId = 340079;

            var pivotHub = new Apollo.PivotalGateway.Harvester(340079);

            Assert.Greater(pivotHub.Iterations.Count(),0);

            Console.WriteLine(pivotHub.Iterations.Count());
            
        }
    }
}
