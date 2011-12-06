using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apollo.PivotalGateway;

namespace ApolloCrawler.Mapping
{
    public class PivotalGatewayAgent
    {
        private static Dictionary<string, Agent> _pivotalAgents =new Dictionary<string, Agent>();

        public static Agent GetPivotalAgent(string name, string url)
        {
            if (_pivotalAgents.ContainsKey(name))
                return _pivotalAgents[name];
            else
            {
                var pivotHub = new Apollo.PivotalGateway.Agent();

                pivotHub.SiteUrl = url;

                var project = pivotHub.Projects.Where(x => x.Name == name).FirstOrDefault();

                if (project != null) pivotHub.ProjectId = project.Id;

                _pivotalAgents.Add(url, pivotHub);

                return pivotHub;
            }
        }
    }
}
