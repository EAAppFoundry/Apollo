using System;
using System.Linq;
using ApolloCrawler.Configuration;
using ApolloCrawler.Mapping;
using Microsoft.Practices.ServiceLocation;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using Microsoft.TeamFoundation.Client;
using SolrNet;
using Project = Microsoft.TeamFoundation.WorkItemTracking.Client.Project;

namespace ApolloCrawler
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                if (args[0].Trim().Equals("dump",StringComparison.CurrentCultureIgnoreCase))
                {
                    if (args.Length == 2)
                    {
                        string url = args[1];

                        DoDumpConfig(url);
                    }

                    return;
                } 
                if (args[0].Trim().Equals("fieldDump", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (args.Length == 3)
                    {
                        int workItemID = int.Parse(args[1]);
                        string url = args[2];

                        DoDumpFields(workItemID, url);
                    }

                    return;
                }
            }

            //load config
            var config = ConfigurationSettings.Load();

            //get solr
            Startup.Init<RequirementsDocument>(config.SolrUrl);
            var solr = ServiceLocator.Current.GetInstance<ISolrOperations<RequirementsDocument>>();
            solr.Commit();

            //get correct mapper for each project
            var mapperFactory = new MapperFactory();

            //foreach project
            foreach (var proj in config.Projects)
            {
                //get the workitems for that project
                var mapper = mapperFactory.GetMapperForProject(proj);
                RequirementsDocument[] docs = mapper.FindAllWorkItemsForProject();

                //submit them to solr
                solr.AddRange(docs, new AddParameters { CommitWithin = 10000 });
            }


            //commit and optimize
            solr.Commit();
            solr.Optimize();
            

        }


        /// <summary>
        /// Dumps all of the fields for 
        /// </summary>
        /// <param name="project"></param>
        /// <param name="url"></param>
        private static void DoDumpFields(int workItemID, string url)
        {
            //connect to TFS
            WorkItemStore wis = WorkItemStoreManager.GetWorkItemStoreForUrl(url);

            var wi = wis.GetWorkItem(workItemID);

            foreach (Field field in wi.Fields)
            {
                Console.WriteLine(field.Name + " := " + field.Value);
            }
        }


        /// <summary>
        /// Print out an example config file of all tfs projects for the given url
        /// </summary>
        /// <param name="url"></param>
        private static void DoDumpConfig(string url)
        {
            //connect to TFS
            WorkItemStore wis = WorkItemStoreManager.GetWorkItemStoreForUrl(url);
            
            //get a list of projects

            Project[] projs = wis.Projects.Cast<Project>().ToArray();
            
            //create a new config
            var config = new ConfigurationSettings();

            //populate it with a dummy solr url
            config.SolrUrl = "!!Your Solr url here!!";

            //put the projects into it
            config.Projects=projs.Where(X => X.HasWorkItemReadRights)
                            .Select(X => new Configuration.Project()
                                             {
                                                 ProjectName = X.Name, 
                                                 SystemType = SystemType.TFS2010, 
                                                 Url = url
                                             }).ToArray();
            
            //serialize it
            string configXml = config.Serialize();

            //print it out to the console
            Console.WriteLine(configXml);

        }

        private static void PopulatePivotalProject(MapperFactory mapperFactory, ISolrOperations<RequirementsDocument> solr)
        {
            Configuration.Project pivotproject = new Configuration.Project() { ProjectName = "Program Mgmt", SystemType = SystemType.Pivotal,Url = @"https://www.pivotaltracker.com/services/v3/" };

            var mapper = mapperFactory.GetMapperForProject(pivotproject);

            RequirementsDocument[] docs = mapper.FindAllWorkItemsForProject();

            //submit them to solr
            solr.AddRange(docs, new AddParameters { CommitWithin = 10000 });

        }       
    }
}