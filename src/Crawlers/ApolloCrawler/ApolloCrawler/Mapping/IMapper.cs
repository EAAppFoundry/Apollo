using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace ApolloCrawler.Mapping
{
    public interface IMapper
    {
        RequirementsDocument[] FindAllWorkItemsForProject();
    }
}
