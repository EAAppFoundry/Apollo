using System.Collections.Generic;
using RestSharp;

namespace Apollo.PivotalGateway
{
    public class Harvester
    {
        #region Constructors

        public Harvester(int projectId)
        {
            ProjectId = projectId;
        }

        #endregion

        #region Properties

        public int ProjectId { get; set; }

        private RestClient _client;


        private void  InitializePivotalRestClient()
        {
            _client = new RestClient("https://www.pivotaltracker.com/services/v3/");
        }
        private IEnumerable<Iteration> _iterations;

        public IEnumerable<Iteration> Iterations
        {
            get { return _iterations ?? (_iterations = GetProjectIterations()); }
        }


        private IEnumerable<Iteration> GetProjectIterations()
        {

            InitializePivotalRestClient();
            
            var request = new PivotalRestRequest("projects/{ProjectId}/iterations", Method.GET);

            request.AddUrlSegment("ProjectId", ProjectId.ToString());

            return _client.Execute<Iterations>(request).Data.Value;

        }

        private Project _project;

        public Project Project
        {
            get { return _project  ?? (_project = GetProjectData());}
        }

        #endregion


        private Project GetProjectData()
        {
            InitializePivotalRestClient();

            var request = new PivotalRestRequest("projects/{ProjectId}", Method.GET);

            request.AddUrlSegment("ProjectId", ProjectId.ToString());

            var project = _client.Execute<Project>(request);

            return project.Data;
        }
    }
}
