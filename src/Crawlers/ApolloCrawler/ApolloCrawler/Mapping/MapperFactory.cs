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
                        return new ScrumMapper(project);
                    case SystemType.TFSUrbanTurtle:
                        return new UrbanTurtleMapper(project);
                    case SystemType.TFS2010Scrum:
                        return new Scrum2010Mapper(project);
                    case SystemType.Pivotal:
                        return new PivotalTrackerMapper();
            }

            return null;
        }
    }
}
