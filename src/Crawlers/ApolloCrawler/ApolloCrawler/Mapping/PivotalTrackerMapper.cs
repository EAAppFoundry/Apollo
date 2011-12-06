using System;
using System.Collections.Generic;
using System.Linq;
using Apollo.PivotalGateway;

namespace ApolloCrawler.Mapping
{
    public class PivotalTrackerMapper : IMapper
    {
        #region IMapper Members


        private string _systemName = "PivotalTracker";
        private string _projectName=null;
        private Apollo.PivotalGateway.Agent _agent;

      

        #endregion



        public PivotalTrackerMapper(string projectName, string url)
        {
            _projectName = projectName;
            _agent = PivotalGatewayAgent.GetPivotalAgent(projectName, url);
        }

        public RequirementsDocument[] FindAllWorkItemsForProject()
        {
            const int batchsize = 10;
            var currentIndex = 1;
            IEnumerable<Story> stories;
            var requirementsDocuments = new List<RequirementsDocument>();

            while ((stories = _agent.Stories(batchsize,currentIndex)) != null)
            {
                requirementsDocuments.AddRange(stories.Select(TranslatePivotalToSolr));
                currentIndex += batchsize; 
            }
            return requirementsDocuments.ToArray();
           
        }

        public RequirementsDocument TranslatePivotalToSolr(Story pivotalstory)
        {
            //return null;

            return new RequirementsDocument()
                       {
                           ID = IDGenerator.GetUniqueIDForDocument(pivotalstory.Id.ToString(), _systemName),
                           Title = pivotalstory.Name,
                           Status = pivotalstory.Current_State,
                           Project = _projectName,
                           Department = "EA",
                           SystemSource = _systemName,
                           LastIndexed = DateTime.Now,
                           Description = pivotalstory.Description,
                           //AcceptanceCriteria = "Accepted At " + pivotalstory.AcceptedAt,
                           StoryPoints = pivotalstory.Estimate.ToString(),
                           ReleaseIteration = "tbd",
                           ReferenceID = "tbd",
                           StoryType = pivotalstory.Story_Type,
                           Team = pivotalstory.Owned_By,
                           //LastUpdated = pivotalstory.UpdatedAt,
                           StoryURI = pivotalstory.Url,
                           StoryTags = pivotalstory.Labels,
                           IterationStart = GetIterationDurationForStory(pivotalstory.Id,pivotalstory.Project_Id),
                           IterationEnd = GetIterationDurationForStory(pivotalstory.Id,pivotalstory.Project_Id)
                       };
        }

        private DateTime GetIterationDurationForStory(int stroyid, int projectId)
        {
            return DateTime.UtcNow;
        }
    }
}
