using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace ApolloCrawler.Mapping
{
    public class ScrumMapper : IMapper
    {
        private string _systemName = "TFS 2008 Scrum";
        private string _projectName=null;
        private WorkItemStore _workItemStore = null;
        private string _url = null;
        private List<Sprint> _allSprints;

        public ScrumMapper(string projectName, string url)
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

            WorkItem[] results = query.Cast<WorkItem>().ToArray();

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
                           Description = wi.Fields["Description"].Value.ToString(),
                           AcceptanceCriteria = wi.Fields["Conditions of Acceptance (Scrum)"].Value.ToString(),
                           StoryPoints = wi.Fields["Estimated Effort (Scrum)"].Value == null ? "" : wi.Fields["Estimated Effort (Scrum)"].Value.ToString(),
                           ReleaseIteration = iterationPath,
                           ReferenceID = wi.Fields["ID"].Value.ToString(),
                           StoryType = "Story",
                           Team = wi.Fields["Team (Scrum)"].Value.ToString(),
                           LastUpdated = DateTime.Parse(wi.Fields["Changed Date"].Value.ToString()),
                           Area = wi.Fields["Area Path"].Value.ToString(),
                           Discipline = "Development",
                           StoryURI = _url + "wi.aspx?id=" + wi.Fields["ID"].Value.ToString()
                           
                       };

            if (sprint!=null)
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
            DateTime? start=null;
            DateTime? end=null;

            if (wi.Fields["Sprint Start (Scrum)"].Value != null)
                start = DateTime.Parse(wi.Fields["Sprint Start (Scrum)"].Value.ToString());

            if (wi.Fields["Sprint End (Scrum)"].Value != null)
                end = DateTime.Parse(wi.Fields["Sprint End (Scrum)"].Value.ToString());
            
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
