using System;
using System.Linq;
using ApolloCrawler.Mapping;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace ApolloCrawler.Mapping
{
    public class UrbanTurtleMapper : IMapper
    {
        private string _systemName = "TFS 2010 Urban Turtle";
        private WorkItemStore _workItemStore = null;
        private Configuration.Project _project = null;

        public UrbanTurtleMapper(Configuration.Project project)
        {
            _project = project;
            _workItemStore = WorkItemStoreManager.GetWorkItemStoreForUrl(_project.Url);
        }

        public RequirementsDocument[] FindAllWorkItemsForProject()
        {
            var query = _workItemStore.Query(@"
                Select [ID], [Iteration Path], [State], [Title], [Description], [Area ID], [Changed Date], [Story Points]
                From WorkItems
                Where 
                ([Work Item Type] = 'User Story')
                    and 
                ([Team Project] = '" + _project.ProjectName + @"')
                    and
                ([State] <> 'Removed')
                Order By [State] Asc, [Changed Date] Desc");

            //TODO: Can't I get some linq from this query?
            WorkItem[] results = new WorkItem[query.Count];

            for (int i = 0; i < query.Count; i++)
                results[i] = query[i];

            return results.Select(X => TranslateTFSToSolr(X)).ToArray();
        }

        private RequirementsDocument TranslateTFSToSolr(WorkItem wi)
        {
            //TODO: Find where iteration start and end is stored. They are not sprint workitems.
            return new RequirementsDocument()
                       {
                           ID = IDGenerator.GetUniqueIDForDocument(wi.Fields["ID"].Value.ToString(),_systemName),
                           Title = wi.Fields["Title"].Value.ToString(),
                           Status = wi.Fields["State"].Value.ToString(),
                           Project = wi.Fields["Team Project"].Value.ToString(),
                           Department = _project.Department,
                           SystemSource = _systemName,
                           LastIndexed = DateTime.Now,
                           Description = wi.Fields["Description"].Value.ToString(),
                           AcceptanceCriteria = "", //doesn't have it
                           Team = wi.Fields["Team Project"].Value.ToString(),
                           Area = wi.Fields["Area Path"].Value.ToString(),
                           Discipline = "Development",
                           ReferenceID = wi.Fields["ID"].Value.ToString(),
                           StoryPoints = wi.Fields["Story Points"].Value == null ? "" : wi.Fields["Story Points"].Value.ToString(),
                           LastUpdated = DateTime.Parse(wi.Fields["Changed Date"].Value.ToString()),
                           ReleaseIteration = wi.Fields["Iteration Path"].Value.ToString(),
                           StoryType = "Story",
                           StoryURI = _project.Url + "/web/UI/Pages/WorkItems/WorkItemEdit.aspx?id=" + wi.Fields["ID"].Value.ToString()
                          
                       };
        }

        
    }
}
