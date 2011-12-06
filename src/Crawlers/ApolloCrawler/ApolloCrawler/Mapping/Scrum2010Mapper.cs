using System;
using System.Linq;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace ApolloCrawler.Mapping
{
    public class Scrum2010Mapper : IMapper
    {
        private string _systemName = "TFS 2010 Scrum";
        private string _projectName=null;
        private WorkItemStore _workItemStore = null;

        public Scrum2010Mapper(string projectName, string url)
        {
            _projectName = projectName;
            _workItemStore = WorkItemStoreManager.GetWorkItemStoreForUrl(url);
        }

        public RequirementsDocument[] FindAllWorkItemsForProject()
        {
            var query = _workItemStore.Query(@"
                Select [ID]
                From WorkItems
                Where 
                ([Work Item Type] = 'Product Backlog Item')
                    and 
                ([Team Project] = '" + _projectName + @"')
                    and
                ([State] <> 'Deleted')
                Order By [State] Asc, [Changed Date] Desc");

            //TODO: Can't I get some linq from this query?
            WorkItem[] results = new WorkItem[query.Count];

            for (int i = 0; i < query.Count; i++)
                results[i] = query[i];

            return results.Select(X => TranslateTFSToSolr(X)).ToArray();
        }

        public RequirementsDocument TranslateTFSToSolr(WorkItem wi)
        {
            return new RequirementsDocument()
                       {
                           ID = IDGenerator.GetUniqueIDForDocument(wi.Fields["ID"].Value.ToString(),_systemName),
                           Title = wi.Fields["Title"].Value.ToString(),
                           Status = wi.Fields["State"].Value.ToString(),
                           Project = wi.Fields["Team Project"].Value.ToString(),
                           Department = "NetOps",
                           SystemSource = _systemName,
                           LastIndexed = DateTime.Now,
                           Description = wi.Fields["Description HTML"].Value.ToString(),
                           AcceptanceCriteria = wi.Fields["Acceptance Criteria"].Value.ToString(),
                           StoryPoints = wi.Fields["Effort"].Value == null ? "" : wi.Fields["Effort"].Value.ToString(),
                           ReleaseIteration = wi.Fields["Iteration Path"].Value.ToString(),
                           ReferenceID = wi.Fields["ID"].Value.ToString(),
                           StoryType = "Story",
                           Team = wi.Fields["Team Project"].Value.ToString(),
                           LastUpdated = DateTime.Parse(wi.Fields["Changed Date"].Value.ToString()),
                           Area = wi.Fields["Area Path"].Value.ToString()
                       };
        }

        
    }
}
