using System;
using System.Linq;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace ApolloTFSCrawler.TFSMapping
{
    public class ScrumMapper : ITFSMapper
    {
        private string _systemName = "TFS 2008 Scrum";
        private string _projectName=null;
        private WorkItemStore _workItemStore = null;

        public ScrumMapper(string projectName, WorkItemStore workItemStore)
        {
            _projectName = projectName;
            _workItemStore = workItemStore;
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
                           Description = wi.Fields["Description"].Value.ToString(),
                           AcceptanceCriteria = wi.Fields["Conditions of Acceptance (Scrum)"].Value.ToString(),
                           StoryPoints = wi.Fields["Estimated Effort (Scrum)"].Value == null ? "" : wi.Fields["Estimated Effort (Scrum)"].Value.ToString(),
                           ReleaseIteration = wi.Fields["Iteration Path"].Value.ToString(),
                           ReferenceID = wi.Fields["ID"].Value.ToString(),
                           StoryType = "Story",
                           Team = wi.Fields["Team (Scrum)"].Value.ToString(),
                           LastUpdated = DateTime.Parse(wi.Fields["Changed Date"].Value.ToString()),
                           Area = wi.Fields["Area Path"].Value.ToString()
                       };
        }

        
    }
}
