using System;
using System.Collections.Generic;
using System.Linq;
using Apollo.PivotalGateway;

namespace ApolloCrawler.Mapping
{
    public class PivotalTrackerMapper : IMapper
    {
        #region IMapper Members


        private Apollo.PivotalGateway.Agent _agent;
        private Configuration.Project _project = null;
      

        #endregion

        public PivotalTrackerMapper(Configuration.Project project)
        {
            _project = project;
            _agent = PivotalGatewayAgent.GetPivotalAgent(project.ProjectName, project.Url);
        }

        public RequirementsDocument[] FindAllWorkItemsForProject()
        {
            const int batchsize = 100;
            var currentIndex = 1;
            IEnumerable<Story> stories;
            var requirementsDocuments = new List<RequirementsDocument>();

            while ((stories = _agent.Stories(batchsize,currentIndex)) != null )
            {
                if (stories.Count() == 0) break;
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
                           ID = IDGenerator.GetUniqueIDForDocument(pivotalstory.Id.ToString(), _project.SystemType.ToDescription()),
                           Title = pivotalstory.Name,
                           Status = pivotalstory.Current_State,
                           Project = _project.ProjectName,
                           Department = _project.Department,
                           SystemSource = _project.SystemType.ToDescription(),
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
