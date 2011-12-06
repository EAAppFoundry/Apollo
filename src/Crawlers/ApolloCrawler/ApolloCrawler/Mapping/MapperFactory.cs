using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace ApolloCrawler.Mapping
{
    public class MapperFactory
    {

        public IMapper GetMapperForProject(Configuration.Project project)
        {
            switch (project.SystemType)
            {
                    case SystemType.TFSScrum:
                        return new ScrumMapper(project.ProjectName, project.Url);
                    case SystemType.TFSUrbanTurtle:
                        return new UrbanTurtleMapper(project.ProjectName, project.Url);
                    case SystemType.TFS2010Scrum:
                        return new Scrum2010Mapper(project.ProjectName, project.Url);
                    case SystemType.Pivotal:
                        return new PivotalTrackerMapper(project.ProjectName,project.Url);
            }

            return null;
        }
    }
}
