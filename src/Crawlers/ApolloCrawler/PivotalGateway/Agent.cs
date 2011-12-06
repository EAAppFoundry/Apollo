using System.Collections.Generic;
using RestSharp;

namespace Apollo.PivotalGateway
{
    public class Agent
    {
        #region Constructors

        public Agent() {}

        public Agent(string projectId)
        {
            ProjectId = projectId;
        }

        #endregion

        #region Properties

        public string SiteUrl { get; set; }

        public string ProjectId { get; set; }

        private RestClient _client;


        private void  InitializePivotalRestClient()
        {
            _client = new RestClient(SiteUrl ?? "https://www.pivotaltracker.com/services/v3/");
        }


        private IEnumerable<Project> _projects;
        public IEnumerable<Project> Projects
        {
            get { return _projects ?? (_projects = GetProjects()); }
        }

        private IEnumerable<Project> GetProjects()
        {

            InitializePivotalRestClient();

            var request = new PivotalRestRequest("projects", Method.GET);

            return _client.Execute<Projects>(request).Data.Value;
        }

        private IEnumerable<Iteration> _iterations;
        public IEnumerable<Iteration> Iterations
        {
            get { return _iterations ?? (_iterations = GetProjectIterations()); }
        }


        public IEnumerable<Story> Stories(int limit, int offset)
        {
            return  GetProjectStories(limit,offset); 
        }


        private IEnumerable<Iteration> GetProjectIterations()
        {

            InitializePivotalRestClient();
            
            var request = new PivotalRestRequest("projects/{ProjectId}/iterations/1", Method.GET);

            request.AddUrlSegment("ProjectId", ProjectId);

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

            request.AddUrlSegment("ProjectId", ProjectId);

            var project = _client.Execute<Project>(request);

            return project.Data;
        }

        private IEnumerable<Story> GetProjectStories(int limit, int offset)
        {
        // "http://www.pivotaltracker.com/services/v3/projects/$PROJECT_ID/stories?limit=10&offset=20"
   

            InitializePivotalRestClient();

            string requestFormat = string.Format("projects/{0}/stories?limit={1}&offset={2}", ProjectId, limit, offset);

            var request = new PivotalRestRequest(requestFormat, Method.GET);

            var myStories = _client.Execute<Stories>(request);

            return myStories.Data.Value;

        }
    }
}
