using System;
using System.Linq;
using ApolloCrawler.Mapping;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace ApolloCrawler.Mapping
{
    public class UrbanTurtleMapper : IMapper
    {
        private string _systemName = "TFS 2010 Urban Turtle";
        private string _projectName=null;
         private WorkItemStore _workItemStore = null;

        public UrbanTurtleMapper(string projectName,  string url)
        {
            _projectName = projectName;
            _workItemStore = WorkItemStoreManager.GetWorkItemStoreForUrl(url);
        }

        public RequirementsDocument[] FindAllWorkItemsForProject()
        {
            var query = _workItemStore.Query(@"
                Select [ID], [Iteration Path], [State], [Title], [Description], [Area ID], [Changed Date], [Story Points]
                From WorkItems
                Where 
                ([Work Item Type] = 'User Story')
                    and 
                ([Team Project] = '" + _projectName + @"')
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
            return new RequirementsDocument()
                       {
                           ID = IDGenerator.GetUniqueIDForDocument(wi.Fields["ID"].Value.ToString(),_systemName),
                           Title = wi.Fields["Title"].Value.ToString(),
                           Status = wi.Fields["State"].Value.ToString(),
                           Project = wi.Fields["Team Project"].Value.ToString(),
                           Department = "NetOps",
                           SystemSource = _systemName,
                           LastIndexed = DateTime.Now,
                           Description = wi.Fields["Description"].Value.ToString()
                       };
        }

        
    }
}
