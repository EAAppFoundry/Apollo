using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace ApolloCrawler.Mapping
{
    public class Scrum2010Mapper : IMapper
    {
        private string _systemName = "TFS 2010 Scrum";
        private string _projectName=null;
        private WorkItemStore _workItemStore = null;
        private List<Sprint> _allSprints;
        private string _url = null;

        public Scrum2010Mapper(string projectName, string url)
        {
            _projectName = projectName;
            _workItemStore = WorkItemStoreManager.GetWorkItemStoreForUrl(url);
            _url = url;
        }

        public RequirementsDocument[] FindAllWorkItemsForProject()
        {
            LoadAllSprints();

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
            string iterationPath = wi.Fields["Iteration Path"].Value.ToString();
            Sprint sprint = _allSprints.Where(X => X.FullPath == iterationPath).FirstOrDefault();

            var result = new RequirementsDocument()
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
                           Area = wi.Fields["Area Path"].Value.ToString(),
                           Discipline = "Development",
                           StoryURI = _url + "/web/UI/Pages/WorkItems/WorkItemEdit.aspx?id=" + wi.Fields["ID"].Value.ToString()
                           
                       };

            if (sprint != null)
            {
                if (sprint.StartDate.HasValue)
                    result.IterationStart = sprint.StartDate.Value;

                if (sprint.EndDate.HasValue)
                    result.IterationEnd = sprint.EndDate.Value;
            }

            return result;
        }

        private void LoadAllSprints()
        {
            var query = _workItemStore.Query(@"
                Select [ID]
                From WorkItems
                Where 
                ([Work Item Type] = 'Sprint')
                    and 
                ([Team Project] = '" + _projectName + @"')
                
                Order By [State] Asc, [Changed Date] Desc");

            _allSprints = query.Cast<WorkItem>()
                .Select(X => TranslateWorkItemToSprint(X))
                .ToList();

        }

        private Sprint TranslateWorkItemToSprint(WorkItem wi)
        {
            DateTime? start = null;
            DateTime? end = null;

            if (wi.Fields["Start Date"].Value != null)
                start = DateTime.Parse(wi.Fields["Start Date"].Value.ToString());

            if (wi.Fields["Finish Date"].Value != null)
                end = DateTime.Parse(wi.Fields["Finish Date"].Value.ToString());

            return new Sprint()
            {
                FullPath = wi.Fields["Iteration Path"].Value.ToString(),
                //TODO: Why doesn't this work?
                //StartDate = wi.Fields["Sprint Start (Scrum)"].Value==null ? null : DateTime.Parse(wi.Fields["Sprint Start (Scrum)"].Value.ToString())),
                StartDate = start,
                EndDate = end
            };
        }


        
    }
}
