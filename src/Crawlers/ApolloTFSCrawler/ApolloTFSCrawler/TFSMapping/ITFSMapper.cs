using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace ApolloTFSCrawler.TFSMapping
{
    public interface ITFSMapper
    {
        RequirementsDocument[] FindAllWorkItemsForProject();
        //RequirementsDocument GetSolrDocumentFromTFSByID(int id, WorkItemStore workItemStore);
    }
}
